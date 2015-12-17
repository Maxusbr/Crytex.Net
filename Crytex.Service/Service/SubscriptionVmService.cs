using System;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models.Biling;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Model.Models;

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
            var os = this._operatingSystemService.GetById(options.OperatingSystemId);
            var tariff = this._tariffInfoService.GetTariffByVirtualization(options.Virtualization, os.Family);
            var subscritionDateEnd = DateTime.UtcNow.AddMonths(options.SubscriptionsMonthCount);
            var newSubscription = new SubscriptionVm
            {
                Id = newTask.GetOptions<CreateVmOptions>().UserVmId,
                AutoDetection = options.AutoDetection,
                DateCreate = DateTime.UtcNow,
                DateEnd = subscritionDateEnd,
                UserId = options.UserId,
                SubscriptionType = options.SubscriptionType,
                TariffId = tariff.Id
            };
            this._subscriptionVmRepository.Add(newSubscription);
            this._unitOfWork.Commit();

            // Calculate subscription price, add a billiing transaction and update user balance
            var tariffMonthPrice = this._tariffInfoService.CalculateTotalPrice(options.Cpu, options.Hdd,
                options.SDD, options.Ram, 0, tariff); // TODO: SDD параметр пока участвует только в билинге. параметр load10percent пока 0
            var transactionCashAmount = tariffMonthPrice * options.SubscriptionsMonthCount;
            var transaction = new BillingTransaction
            {
                SubscriptionVmId = newSubscription.Id,
                CashAmount = -transactionCashAmount,
                TransactionType = BillingTransactionType.OneTimeDebiting,
                SubscriptionVmMonthCount = options.SubscriptionsMonthCount,
                UserId = options.UserId 
            };
            this._billingService.AddUserTransaction(transaction);

            return newSubscription;
        }
    }
}
