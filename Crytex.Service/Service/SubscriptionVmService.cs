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
using System.Globalization;
using Crytex.Core.Extension;
using System.Linq;
using Crytex.Model.Enums;

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
        private readonly IUsageSubscriptionPaymentRepository _usageSubscriptionPaymentRepo;
        private readonly IFixedSubscriptionPaymentRepository _fixedSubscriptionPaymentRepo;
        private readonly ISubscriptionBackupPaymentRepository _backupPaymentRepo;

        public SubscriptionVmService(IUnitOfWork unitOfWork, ISubscriptionVmRepository subscriptionVmRepository, ITaskV2Service taskService,
            IBilingService billingService, ITariffInfoService tariffInfoService, IOperatingSystemsService operatingSystemService,
            IUsageSubscriptionPaymentRepository usageSubscriptionPaymentRepo, IFixedSubscriptionPaymentRepository fixedSubscriptionPaymentRepo,
            ISubscriptionBackupPaymentRepository backupPaymentRepo)
        {
            this._unitOfWork = unitOfWork;
            this._subscriptionVmRepository = subscriptionVmRepository;
            this._taskService = taskService;
            this._billingService = billingService;
            this._tariffInfoService = tariffInfoService;
            this._operatingSystemService = operatingSystemService;
            this._usageSubscriptionPaymentRepo = usageSubscriptionPaymentRepo;
            this._fixedSubscriptionPaymentRepo = fixedSubscriptionPaymentRepo;
            this._backupPaymentRepo = backupPaymentRepo;
        }

        public SubscriptionVm BuySubscription(SubscriptionBuyOptions options)
        {
            SubscriptionVm newSubscription = null;

            switch (options.SubscriptionType)
            {
                case SubscriptionType.Fixed:
                    newSubscription = this.BuyFixedSubscription(options);
                    break;
                case SubscriptionType.Usage:
                    newSubscription = this.BuyUsageSubscription(options);
                    break;
            }

            return newSubscription;
        }

        private SubscriptionVm BuyFixedSubscription(SubscriptionBuyOptions options)
        {
            // Calculate subscription price, add a billiing transaction and update user balance
            var os = this._operatingSystemService.GetById(options.OperatingSystemId);
            var tariff = this._tariffInfoService.GetTariffByType(options.Virtualization, os.Family);
            decimal transactionCashAmount = 0;
            decimal backupTransactionCashAmount = 0;
            var backupPaymentRequired = options.DailyBackupStorePeriodDays > 1;
            if (!options.BoughtByAdmin)
            {
                var tariffMonthPrice = this._tariffInfoService.CalculateTotalPrice(options.Cpu, options.Hdd,
                options.SDD, options.Ram, 0, tariff); // TODO: SDD параметр пока участвует только в билинге. параметр load10percent пока 0
                transactionCashAmount = tariffMonthPrice * options.SubscriptionsMonthCount;

                if (backupPaymentRequired)
                {
                    backupTransactionCashAmount = this._tariffInfoService.CalculateBackupPrice(options.Hdd, options.SDD, options.DailyBackupStorePeriodDays - 1, tariff);
                }
            }
            
            var subsciptionVmTransaction = new BillingTransaction
            {
                CashAmount = -(transactionCashAmount + backupTransactionCashAmount),
                TransactionType = BillingTransactionType.OneTimeDebiting,
                SubscriptionVmMonthCount = options.SubscriptionsMonthCount,
                UserId = options.UserId,
                AdminUserId = options.AdminUserId
            };
            subsciptionVmTransaction = this._billingService.AddUserTransaction(subsciptionVmTransaction);

            // Create task and new vm
            var newSubscription = this.PrepareNewSubscription(options, tariff);
            
            // Add new sub payment
            var subscriptionPayment = new FixedSubscriptionPayment
            {
                BillingTransactionId = subsciptionVmTransaction.Id,
                MonthCount = options.SubscriptionsMonthCount,
                Date = DateTime.UtcNow,
                DateStart = newSubscription.DateCreate,
                DateEnd = newSubscription.DateEnd,
                SubscriptionVmId = newSubscription.Id,
                CoreCount = options.Cpu,
                HardDriveSize = options.Hdd,
                RamCount = options.Ram,
                Amount = transactionCashAmount,
                TariffId = newSubscription.TariffId
            };
            this._fixedSubscriptionPaymentRepo.Add(subscriptionPayment);
            this._unitOfWork.Commit();

            // Add new sub backup payment
            var subscriptionBackupPayment = new SubscriptionVmBackupPayment
            {
                BillingTransactionId = subsciptionVmTransaction.Id,
                Date = DateTime.UtcNow,
                SubscriptionVmId = newSubscription.Id,
                Amount = backupTransactionCashAmount,
                TariffId = newSubscription.TariffId,
                DaysPeriod = options.DailyBackupStorePeriodDays
            };
            this._backupPaymentRepo.Add(subscriptionBackupPayment);
            this._unitOfWork.Commit();

            // Update SubscriptionVmId
            this._billingService.UpdateTransactionSubscriptionId(subsciptionVmTransaction.Id, newSubscription.Id);

            return newSubscription;
        }

        private SubscriptionVm BuyUsageSubscription(SubscriptionBuyOptions options)
        {
            // Calculate subscription hour price, add a billiing transaction and update user balance
            var os = this._operatingSystemService.GetById(options.OperatingSystemId);
            var tariff = this._tariffInfoService.GetTariffByType(options.Virtualization, os.Family);

            decimal tariffHourPrice;
            if (!options.BoughtByAdmin)
            {
                tariffHourPrice = this._tariffInfoService.CalculateTotalPrice(options.Cpu, options.Hdd,
                options.SDD, options.Ram, 0, tariff) / (30 * 24); // TODO: SDD параметр пока участвует только в билинге. параметр load10percent пока 0
            }
            else
            {
                tariffHourPrice = 0;
            }

            var transaction = new BillingTransaction
            {
                CashAmount = -tariffHourPrice,
                TransactionType = BillingTransactionType.OneTimeDebiting,
                UserId = options.UserId,
                AdminUserId = options.AdminUserId
            };
            var newTransaction = this._billingService.AddUserTransaction(transaction);
            
            // Create task and new vm with
            var newSubscription = this.PrepareNewSubscription(options, tariff);

            var subscriptionPayment = new UsageSubscriptionPayment
            {
                BillingTransactionId = newTransaction.Id,
                Date = DateTime.UtcNow,
                SubscriptionVmId = newSubscription.Id,
                CoreCount = options.Cpu,
                HardDriveSize = options.Hdd,
                RamCount = options.Ram,
                Amount = tariffHourPrice,
                TariffId = newSubscription.TariffId
            };
            this._usageSubscriptionPaymentRepo.Add(subscriptionPayment);
            this._unitOfWork.Commit();

            // Update SubscriptionVmId
            this._billingService.UpdateTransactionSubscriptionId(newTransaction.Id, newSubscription.Id);

            return newSubscription;
        }

        private SubscriptionVm PrepareNewSubscription(SubscriptionBuyOptions options, Tariff tariff)
        {
            var createVmOptions = new CreateVmOptions
            {
                Cpu = options.Cpu,
                Hdd = options.Hdd,
                Ram = options.Ram,
                OperatingSystemId = options.OperatingSystemId,
                Name = options.VmName
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
            var date = DateTime.Now;
            var newSubscription = new SubscriptionVm
            {
                Id = newTask.GetOptions<CreateVmOptions>().UserVmId,
                AutoProlongation = options.AutoProlongation,
                DateCreate = DateTime.UtcNow,
                DateEnd = subscritionDateEnd,
                UserId = options.UserId,
                Name = options.VmName,
                SubscriptionType = options.SubscriptionType,
                TariffId = tariff.Id,
                Status = SubscriptionVmStatus.Active,
                LastUsageBillingTransactionDate = DateTime.UtcNow.TrimToGraterHour(),
                DailyBackupStorePeriodDays = options.DailyBackupStorePeriodDays
            };
            this._subscriptionVmRepository.Add(newSubscription);
            this._unitOfWork.Commit();

            return newSubscription;
        }

        public virtual SubscriptionVm GetById(Guid guid)
        {
            var sub = this._subscriptionVmRepository.Get(s => s.Id == guid, s => s.UserVm.OperatingSystem, s => s.User);

            if(sub == null)
            {
                throw new InvalidIdentifierException($"Vm subscription with id={guid.ToString()} doesn't exist");
            }

            return sub;
        }

        public IEnumerable<SubscriptionVm> GetAllByStatus(SubscriptionVmStatus status)
        {
            var subs = this._subscriptionVmRepository.GetMany(x => x.Status == status);

            return subs;
        }

        public IPagedList<UsageSubscriptionPayment> GetPageUsageSubscriptionPayment(int pageNumber, int pageSize, string userId = null, UsageSubscriptionPaymentSearchParams searchParams = null)
        {
            var pageInfo = new PageInfo(pageNumber, pageSize);

            Expression<Func<UsageSubscriptionPayment, bool>> where = x => x.SubscriptionVm.SubscriptionType == SubscriptionType.Usage;

            if (searchParams != null)
            {
                where = CreateWhereExpression(userId, searchParams);
            }

            var pagedList = this._usageSubscriptionPaymentRepo
                .GetPage(pageInfo, where, s => s.Date, false, s => s.SubscriptionVm, s => s.SubscriptionVm.UserVm,
                    s => s.SubscriptionVm.User);

            return pagedList;
        }

        public StaticPagedList<UsageSubscriptionPaymentContainer> GetPageUsageSubscriptionPaymentByPeriod(int pageNumber, int pageSize, string userId = null, UsageSubscriptionPaymentSearchParams searchParams = null)
        {
            var pageInfo = new PageInfo(pageNumber, pageSize);

            if (searchParams.PeriodType == null)
            {
                throw new InvalidIdentifierException("Period is empty");
            }

            Expression<Func<UsageSubscriptionPayment, bool>> where = CreateWhereExpression(userId, searchParams);

            var allPayments = this._usageSubscriptionPaymentRepo.GetMany(where, s => s.SubscriptionVm,
                s => s.SubscriptionVm.UserVm, s => s.SubscriptionVm.User);

            var paymentGroups = GetGroupedPagedList(allPayments, searchParams.PeriodType, pageNumber, pageSize);
            return new StaticPagedList<UsageSubscriptionPaymentContainer>(paymentGroups.ToList(), pageNumber, pageSize, paymentGroups.Count());

        }

        public StaticPagedList<UsageSubscriptionPaymentGroupByVmContainer> GetPageUsageSubscriptionPaymentByVmPeriod(int pageNumber, int pageSize, string userId = null, UsageSubscriptionPaymentSearchParams searchParams = null)
        {
            var pageInfo = new PageInfo(pageNumber, pageSize);

            if (searchParams.PeriodType == null)
            {
                throw new InvalidIdentifierException("Period is empty");
            }

            Expression<Func<UsageSubscriptionPayment, bool>> where = CreateWhereExpression(userId, searchParams);

            var allPayments = this._usageSubscriptionPaymentRepo.GetMany(where, s => s.SubscriptionVm,
                s => s.SubscriptionVm.UserVm, s => s.SubscriptionVm.User);

            var paymentGroupsVm = allPayments.GroupBy(p => p.SubscriptionVm);

            List<UsageSubscriptionPaymentGroupByVmContainer> paymentGroupsByVm = new List<UsageSubscriptionPaymentGroupByVmContainer>();
            foreach (var payment in paymentGroupsVm)
            {
                var group = GetGroupedPagedList(allPayments, searchParams.PeriodType, pageNumber, pageSize);
                paymentGroupsByVm.Add(new UsageSubscriptionPaymentGroupByVmContainer { Name = payment.Key.UserVm.Name, Subscriptions = group });
            }
            return new StaticPagedList<UsageSubscriptionPaymentGroupByVmContainer>(paymentGroupsByVm, pageNumber, pageSize, paymentGroupsByVm.Count());
        }

        public Expression<Func<UsageSubscriptionPayment, bool>> CreateWhereExpression(string userId = null, UsageSubscriptionPaymentSearchParams searchParams = null)
        {
            Expression<Func<UsageSubscriptionPayment, bool>> where = x => x.SubscriptionVm.SubscriptionType == SubscriptionType.Usage;

            if (userId != null)
            {
                where = where.And(x => x.SubscriptionVm.UserId == userId);
            }

            if (searchParams.FromDate != null)
            {
                where = where.And(x => x.Date >= searchParams.FromDate);
            }

            if (searchParams.ToDate != null)
            {
                where = where.And(x => x.Date <= searchParams.ToDate);
            }

            if (searchParams.SubscriptionVmId != null)
            {
                where = where.And(x => x.SubscriptionVmId == searchParams.SubscriptionVmId);
            }

            if (searchParams.TypeVirtualization != null)
            {
                where = where.And(x => x.SubscriptionVm.UserVm.VirtualizationType == searchParams.TypeVirtualization);
            }

            return where;
        }

        public IEnumerable<UsageSubscriptionPaymentContainer> GetGroupedPagedList(List<UsageSubscriptionPayment> allPayments, CountingPeriodType? period, int pageNumber, int pageSize)
        {
            IEnumerable<UsageSubscriptionPaymentContainer> paymentGroups;

            if (period == CountingPeriodType.Day)
            {
                paymentGroups = allPayments.GroupBy(s => new { s.Date.Year, s.Date.Month, s.Date.Day },
                (key, ss) => new UsageSubscriptionPaymentContainer() { Date = new DateTime(key.Year, key.Month, key.Day), UsageSubscriptionPayment = ss })
                .Skip((pageNumber - 1) * pageSize).Take(pageSize);
                return paymentGroups;
            }
            
            paymentGroups = allPayments.GroupBy(s => new { s.Date.Year, s.Date.Month },
            (key, ss) => new UsageSubscriptionPaymentContainer() { Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(key.Month), UsageSubscriptionPayment = ss })
            .Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return paymentGroups;
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

            var pagedList = this._subscriptionVmRepository.GetPage(pageInfo, where, x => x.DateCreate, false, x=>x.User, x=>x.UserVm.OperatingSystem);

            return pagedList;
        }

        public IEnumerable<SubscriptionVm> GetSubscriptionsByStatusAndType(SubscriptionVmStatus status, SubscriptionType type)
        {
            var subs = this._subscriptionVmRepository.GetMany(s => s.Status == status && s.SubscriptionType == type
                && s.UserVm.Status != StatusVM.Creating && s.UserVm.Status != StatusVM.Error);

            return subs;
        }

        public void UpdateSubscriptionData(SubscriptionUpdateOptions model)
        {
            var subscription = this.GetById(model.Id);

            if (subscription.SubscriptionType == SubscriptionType.Fixed)
            {
                subscription.AutoProlongation = model.AutoProlongation;
            }

            subscription.Name = model.Name;
            this._subscriptionVmRepository.Update(subscription);
            this._unitOfWork.Commit();
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

        public void AutoProlongateFixedSubscription(Guid subId)
        {
            try
            {
                this.ProlongateFixedSubscriptionInner(subId, 1, BillingTransactionType.AutomaticDebiting);
            }
            catch (TransactionFailedException)
            {
                this.UpdateSubscriptionStatus(subId, SubscriptionVmStatus.WaitForPayment);
            }
        }

        public void ProlongateFixedSubscription(SubscriptionProlongateOptions options)
        {
            this.ProlongateFixedSubscriptionInner(options.SubscriptionVmId.Value, options.MonthCount.Value,
                BillingTransactionType.OneTimeDebiting, options.ProlongatedByAdmin, options.AdminUserId);
        }

        private void ProlongateFixedSubscriptionInner(Guid subId, int monthCount, BillingTransactionType transactionType,
            bool forFree = false, string adminUserId = null)
        {
            var sub = this.GetById(subId);
            if(sub.SubscriptionType != SubscriptionType.Fixed)
            {
                throw new OperationNotSupportedException($"Cannot prolongate subscription of type {sub.SubscriptionType}");
            }

            decimal totalPrice = 0;
            if (!forFree)
            {
                var tariff = this._tariffInfoService.GetTariffById(sub.TariffId);
                var tariffMonthPrice = this._tariffInfoService.CalculateTotalPrice(sub.UserVm.CoreCount, sub.UserVm.HardDriveSize,
                    0, sub.UserVm.RamCount, 0, tariff); // TODO: SDD параметр 0. loadPer10Percent = 0
                totalPrice = tariffMonthPrice * monthCount;
            }

            var transaction = new BillingTransaction
            {
                SubscriptionVmId = sub.Id,
                CashAmount = -totalPrice,
                TransactionType = transactionType,
                SubscriptionVmMonthCount = monthCount,
                UserId = sub.UserId,
                AdminUserId = adminUserId
            };

            var newTransaction = this._billingService.AddUserTransaction(transaction);

            var subPayment = new FixedSubscriptionPayment
            {
                Amount = totalPrice,
                BillingTransactionId = newTransaction.Id,
                SubscriptionVmId = sub.Id,
                TariffId = sub.TariffId,
                Date = DateTime.UtcNow,
                DateStart = sub.DateEnd.AddTicks(1),
                DateEnd = sub.DateEnd.AddMonths(monthCount),
                CoreCount = sub.UserVm.CoreCount,
                HardDriveSize = sub.UserVm.HardDriveSize,
                RamCount = sub.UserVm.RamCount,
                MonthCount = monthCount
            };
            this._fixedSubscriptionPaymentRepo.Add(subPayment);
            this._unitOfWork.Commit();

            var newSubEndDate = sub.DateEnd.AddMonths(monthCount);
            this.UpdateSubscriptionStatus(sub.Id, SubscriptionVmStatus.Active, newSubEndDate);
        }

        public void PrepareSubscriptionForDeletion(Guid subId)
        {
            var sub = this.GetById(subId);

            // Create changeStatus task
            var removeVmOptions = new ChangeStatusOptions
            {
                VmId = sub.UserVm.Id,
                TypeChangeStatus = TypeChangeStatus.PowerOff
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

            if(sub.Status == SubscriptionVmStatus.Deleted)
            {
                throw new InvalidOperationApplicationException("Subscription is already deleted");
            }

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

            //Return money
            if(sub.SubscriptionType == SubscriptionType.Fixed && sub.DateEnd > DateTime.UtcNow)
            {
                var payments = this._fixedSubscriptionPaymentRepo.GetMany(p => p.SubscriptionVmId == sub.Id);

                // Find all unspended payments and current payment
                // All unspended payments which are not "active" yet (it's startdate is grater than now) will be returned completely
                var futurePayments = payments.Where(p => p.DateStart > DateTime.UtcNow);
                // Find current payment (it's startdate is less and enddate is grater than now)
                // This payment will be returned partially
                var currentPayment = payments.SingleOrDefault(p => p.DateStart <= DateTime.UtcNow && p.DateEnd > DateTime.UtcNow);

                decimal sumToReturn = 0;
                sumToReturn = futurePayments.Sum(p => p.Amount);
                if(currentPayment != null)
                {
                    // Calculate part of current payment to return
                    var totalDays = (currentPayment.DateEnd - currentPayment.DateStart).Days;
                    var pastDays = (DateTime.UtcNow - currentPayment.DateStart).Days;
                    var futureDays = totalDays - pastDays;
                    sumToReturn += (currentPayment.Amount / totalDays) * futureDays;
                }

                var transaction = new BillingTransaction
                {
                    SubscriptionVmId = sub.Id,
                    CashAmount = sumToReturn,
                    TransactionType = BillingTransactionType.Crediting,
                    UserId = sub.UserId,
                    Description = "Return money for deleted subscription"
                };
                this._billingService.AddUserTransaction(transaction);
            }
        }

        public IEnumerable<SubscriptionVm> GetAllFixedSubscriptions()
        {
            var subs = this._subscriptionVmRepository.GetAll(s => s.SubscriptionType == SubscriptionType.Fixed);

            return subs;
        }

        public void UpdateUsageSubscriptionBalance(Guid subId)
        {
            var sub = this.GetById(subId);
            var subTariff = this._tariffInfoService.GetTariffById(sub.TariffId);
            var subVm = sub.UserVm;

            // Calculate usage hour price
            decimal hourPrice;
            if (subVm.Status == StatusVM.Enable)
            {
                hourPrice = this._tariffInfoService.CalculateTotalPrice(subVm.CoreCount,
                    subVm.HardDriveSize, 0, subVm.RamCount, 0, subTariff) / (30 * 24);
            }
            else if(subVm.Status == StatusVM.Disable)
            {
                hourPrice = this._tariffInfoService.CalculateTotalPrice(0, subVm.HardDriveSize,
                    0, 0, 0, subTariff) / (30 * 24);
            }
            else
            {
                throw new ApplicationException($"Machine status during updating usage-type vm subscription is {subVm.Status}");
            }

            // Calculate hour count for payment
            var currentTime = DateTime.UtcNow;
            var intHours = (currentTime - sub.LastUsageBillingTransactionDate.Value).Hours;
            
            // Create transaction on each hour
            for(int i = 0; i < intHours; i++)
            {
                var hourTransaction = new BillingTransaction
                {
                    TransactionType = BillingTransactionType.AutomaticDebiting,
                    CashAmount = -hourPrice,
                    UserId = sub.UserId,
                    Description = "Hourly debiting for usage-type vm subscription",
                    SubscriptionVmId = sub.UserVm.Id
                };

                var newPayment = new UsageSubscriptionPayment
                {
                    SubscriptionVmId = sub.Id,
                    TariffId = subTariff.Id,
                    Amount = hourPrice,
                    Date = currentTime,
                    RamCount = subVm.RamCount,
                    CoreCount = subVm.CoreCount,
                    HardDriveSize = subVm.HardDriveSize
                };

                try
                {
                    var newTransaction = this._billingService.AddUserTransaction(hourTransaction);

                    newPayment.BillingTransactionId = newTransaction.Id;
                    newPayment.Paid = true;
                }
                catch (TransactionFailedException)
                {
                    newPayment.BillingTransactionId = null;
                    newPayment.Paid = false;
                }

                sub.LastUsageBillingTransactionDate += TimeSpan.FromHours(1);
                this._subscriptionVmRepository.Update(sub);

                this._usageSubscriptionPaymentRepo.Add(newPayment);
                this._unitOfWork.Commit();
            }
        }

        public void StartSubscriptionMachine(Guid subsciptionId)
        {
            this.ChangeSubscrioptionMachineStatus(subsciptionId, TypeChangeStatus.Start);
        }

        public void StopSubscriptionMachine(Guid subsciptionId)
        {
            this.ChangeSubscrioptionMachineStatus(subsciptionId, TypeChangeStatus.Stop);
        }

        public void PowerOffSubscriptionMachine(Guid subsciptionId)
        {
            this.ChangeSubscrioptionMachineStatus(subsciptionId, TypeChangeStatus.PowerOff);
        }

        public void ResetSubscriptionMachine(Guid subsciptionId)
        {
            this.ChangeSubscrioptionMachineStatus(subsciptionId, TypeChangeStatus.Reload);
        }

        private void ChangeSubscrioptionMachineStatus(Guid subscriptionId, TypeChangeStatus status)
        {
            var sub = this.GetById(subscriptionId);

            if (sub.Status != SubscriptionVmStatus.Active)
            {
                throw new InvalidOperationApplicationException("Cannot start subscription's vm. Subscription status is not Active");
            }
            else
            {
                var taskOptions = new ChangeStatusOptions
                {
                    TypeChangeStatus = status,
                    VmId = sub.UserVm.Id
                };
                var task = new TaskV2
                {
                    TypeTask = TypeTask.ChangeStatus,
                    Virtualization = sub.UserVm.VirtualizationType,
                    UserId = sub.UserId
                };

                this._taskService.CreateTask(task, taskOptions);
            }
        }

        public void UpdateSubscriptionMachineConfig(Guid subscriptionId, UpdateMachineConfigOptions options)
        {
            var sub = this.GetById(subscriptionId);

            switch (sub.SubscriptionType)
            {
                case SubscriptionType.Fixed:
                    this.UpdateFixedSubMachineConfig(sub, options);
                    break;
                case SubscriptionType.Usage:
                    this.UpdateUsageSubMachineConfig(sub, options);
                    break;
            }
        }

        private void UpdateUsageSubMachineConfig(SubscriptionVm sub, UpdateMachineConfigOptions options)
        {
            var updateTask = new TaskV2
            {
                TypeTask = TypeTask.UpdateVm,
                UserId = sub.UserId,
                Virtualization = sub.UserVm.VirtualizationType
            };
            var taskUpdateOptions = new UpdateVmOptions
            {
                Cpu = options.Cpu ?? sub.UserVm.CoreCount,
                Hdd = options.Hdd ?? sub.UserVm.HardDriveSize,
                Ram = options.Ram ?? sub.UserVm.RamCount,
                Name = sub.UserVm.Name,
                VmId = sub.UserVm.Id
            };
            this._taskService.CreateTask(updateTask, taskUpdateOptions);
        }

        private void UpdateFixedSubMachineConfig(SubscriptionVm sub, UpdateMachineConfigOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
