using System;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models.Biling;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Model.Models;
using Crytex.Service.Models;
using PagedList;
using System.Linq.Expressions;
using Crytex.Service.Extension;
using Crytex.Model.Exceptions;
using System.Collections.Generic;

namespace Crytex.Service.Service
{
    public class SubscriptionVmService : ISubscriptionVmService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubscriptionVmRepository _subscriptionVmRepository;
        private readonly ITaskV2Service _taskService;
        private readonly IBilingService _billingService;
        private readonly ITariffInfoService _tariffInfoService;
        private readonly IOperatingSystemsService _operatingSystemService;

        public SubscriptionVmService(IUnitOfWork unitOfWork, ISubscriptionVmRepository subscriptionVmRepository, ITaskV2Service taskService,
            IBilingService billingService, ITariffInfoService tariffInfoService, IOperatingSystemsService operatingSystemService)
        {
            this._unitOfWork = unitOfWork;
            this._subscriptionVmRepository = subscriptionVmRepository;
            this._taskService = taskService;
            this._billingService = billingService;
            this._tariffInfoService = tariffInfoService;
            this._operatingSystemService = operatingSystemService;
        }

        public SubscriptionVm BuySubscription(SubscriptionBuyOptions options)
        {
            // Calculate subscription price, add a billiing transaction and update user balance
            var os = this._operatingSystemService.GetById(options.OperatingSystemId);
            var tariff = this._tariffInfoService.GetTariffByType(options.Virtualization, os.Family);
            var tariffMonthPrice = this._tariffInfoService.CalculateTotalPrice(options.Cpu, options.Hdd,
                options.SDD, options.Ram, 0, tariff); // TODO: SDD параметр пока участвует только в билинге. параметр load10percent пока 0
            var transactionCashAmount = tariffMonthPrice * options.SubscriptionsMonthCount;
            var transaction = new BillingTransaction
            {
                CashAmount = -transactionCashAmount,
                TransactionType = BillingTransactionType.OneTimeDebiting,
                SubscriptionVmMonthCount = options.SubscriptionsMonthCount,
                UserId = options.UserId
            };
            var newTransaction = this._billingService.AddUserTransaction(transaction);

            // Create task and new vm with ITaskV2service
            var createVmOptions = new CreateVmOptions
            {
                Cpu = options.Cpu,
                Hdd = options.Hdd,
                Ram = options.Ram,
                OperatingSystemId = options.OperatingSystemId
            };
            var createTask = new TaskV2
            {
                TypeTask = TypeTask.CreateVm,
                Virtualization = options.Virtualization,
                UserId = options.UserId
            };
            var newTask = this._taskService.CreateTask(createTask, createVmOptions);

            // Create new subscription
            var subscritionDateEnd = DateTime.UtcNow.AddMonths(options.SubscriptionsMonthCount);
            var newSubscription = new SubscriptionVm
            {
                Id = newTask.GetOptions<CreateVmOptions>().UserVmId,
                AutoProlongation = options.AutoProlongation,
                DateCreate = DateTime.UtcNow,
                DateEnd = subscritionDateEnd,
                UserId = options.UserId,
                SubscriptionType = options.SubscriptionType,
                TariffId = tariff.Id,
                Status = SubscriptionVmStatus.Active
            };
            this._subscriptionVmRepository.Add(newSubscription);
            this._unitOfWork.Commit();

            // Update SubscriptionVmId
            this._billingService.UpdateTransactionSubscriptionId(newTransaction.Id, newSubscription.Id);

            return newSubscription;
        }

        public virtual SubscriptionVm GetById(Guid guid)
        {
            var sub = this._subscriptionVmRepository.Get(s => s.Id == guid, s => s.UserVm, s => s.User);

            if(sub == null)
            {
                throw new InvalidIdentifierException($"Vm subscription with id={guid.ToString()} doesn't exist");
            }

            return sub;
        }

        public IPagedList<SubscriptionVm> GetPage(int pageNumber, int pageSize, string userId = null, SubscriptionVmSearchParams searchParams = null)
        {
            var pageInfo = new PageInfo(pageNumber, pageSize);

            Expression<Func<SubscriptionVm, bool>> where = x => true;

            if (searchParams != null)
            {
                if (searchParams.CreatedFrom != null)
                {
                    where = where.And(x => x.DateCreate >= searchParams.CreatedFrom);
                }

                if (searchParams.CreatedTo != null)
                {
                    where = where.And(x => x.DateCreate <= searchParams.CreatedTo);
                }

                if (searchParams.EndFrom != null)
                {
                    where = where.And(x => x.DateEnd >= searchParams.EndFrom);
                }
                if(searchParams.EndTo != null)
                {
                    where = where.And(x => x.DateEnd <= searchParams.EndTo);
                }

                if(userId != null)
                {
                    where = where.And(x => x.UserId == userId);
                }
            }

            var pagedList = this._subscriptionVmRepository.GetPage(pageInfo, where, x => x.DateCreate);

            return pagedList;
        }

        public IEnumerable<SubscriptionVm> GetSubscriptionsByStatusAndType(SubscriptionVmStatus status, SubscriptionType type)
        {
            var subs = this._subscriptionVmRepository.GetMany(s => s.Status == status && s.SubscriptionType == type);

            return subs;
        }

        public void UpdateSubscriptionStatus(Guid subId, SubscriptionVmStatus status, DateTime? subEndDate = null)
        {
            var sub = this.GetById(subId);
            sub.Status = status;
            if(subEndDate != null)
            {
                sub.DateEnd = subEndDate.Value;
            }
            this._subscriptionVmRepository.Update(sub);
            this._unitOfWork.Commit();
        }

        public void AutoProlongateSubscription(Guid subId)
        {
            var sub = this.GetById(subId);

            var tariff = this._tariffInfoService.GetTariffById(sub.TariffId);
            var tariffMonthPrice = this._tariffInfoService.CalculateTotalPrice(sub.UserVm.CoreCount, sub.UserVm.HardDriveSize,
                0, sub.UserVm.RamCount, 0, tariff); // TODO: SDD параметр 0. loadPer10Percent = 0

            var transaction = new BillingTransaction
            {
                SubscriptionVmId = sub.Id,
                CashAmount = -tariffMonthPrice,
                TransactionType = BillingTransactionType.OneTimeDebiting,
                SubscriptionVmMonthCount = 1,
                UserId = sub.UserId,
            };

            try
            {
                this._billingService.AddUserTransaction(transaction);
                var newSubEndDate = sub.DateEnd.AddMonths(1);
                this.UpdateSubscriptionStatus(sub.Id, SubscriptionVmStatus.Active, newSubEndDate);
            }
            catch (TransactionFailedException)
            {
                this.UpdateSubscriptionStatus(sub.Id, SubscriptionVmStatus.WaitForPayment);
            }
        }

        public void PrepareSubscriptionForDeletion(Guid subId)
        {
            var sub = this.GetById(subId);

            // Create changeStatus task
            var removeVmOptions = new ChangeStatusOptions
            {
                VmId = sub.UserVm.Id,
                TypeChangeStatus = TypeChangeStatus.PowerOf
            };
            var deleteTask = new TaskV2
            {
                Virtualization = sub.UserVm.VirtualizationType,
                UserId = sub.UserId,
                TypeTask = TypeTask.ChangeStatus
            };
            this._taskService.CreateTask(deleteTask, removeVmOptions);

            // Change subscription status to WaitForDeletion
            sub.Status = SubscriptionVmStatus.WaitForDeletion;
            this._subscriptionVmRepository.Update(sub);
            this._unitOfWork.Commit();
        }

        public void DeleteSubscription(Guid subId)
        {
            var sub = this.GetById(subId);

            // Create removeVm task
            var removeVmOptions = new RemoveVmOptions
            {
                VmId = sub.UserVm.Id
            };
            var deleteTask = new TaskV2
            {
                Virtualization = sub.UserVm.VirtualizationType,
                UserId = sub.UserId,
                TypeTask = TypeTask.RemoveVm
            };
            this._taskService.CreateTask(deleteTask, removeVmOptions);

            // Change subscription status to WaitForDeletion
            sub.Status = SubscriptionVmStatus.Deleted;
            this._subscriptionVmRepository.Update(sub);
            this._unitOfWork.Commit();
        }

        public IEnumerable<SubscriptionVm> GetAllFixedSubscriptions()
        {
            var subs = this._subscriptionVmRepository.GetAll(s => s.SubscriptionType == SubscriptionType.Fixed);

            return subs;
        }
    }
}
