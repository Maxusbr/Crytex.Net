using System;
using Crytex.Service.IService;
using Crytex.Model.Models.WebHostingModels;
using Crytex.Model.Models.Biling;
using Crytex.Data.IRepository;
using Crytex.Data.Infrastructure;
using Crytex.Model.Models;
using Crytex.Service.Model;

namespace Crytex.Service.Service
{
    class WebHostingService : IWebHostingService
    {
        private readonly IBilingService _billingService;
        private readonly IWebHostingPaymentRepository _webHostingPaymentRepository;
        private readonly ITaskV2Service _taskService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostingRepository _webHostingRepository;
        private readonly IWebHostingTariffService _webHostingTariffService;

        public WebHostingService(IWebHostingTariffService webHostingTariffService, IBilingService billingService,
            IWebHostingRepository webHostingRepository, ITaskV2Service taskService, IWebHostingPaymentRepository webHostingPaymentRepo,
            IUnitOfWork unitOfWork)
        {
            this._webHostingTariffService = webHostingTariffService;
            this._billingService = billingService;
            this._webHostingRepository = webHostingRepository;
            this._taskService = taskService;
            this._unitOfWork = unitOfWork;
            this._webHostingPaymentRepository = webHostingPaymentRepo;
        }

        public WebHosting BuyNewHosting(BuyWebHostingParams buyParams)
        {
            var hostingTariff = this._webHostingTariffService.GetById(buyParams.WebHostingTariffId);
            var totalPrice = hostingTariff.Price * buyParams.MonthCount;
            var hostingBuyTransaction = new BillingTransaction
            {
                CashAmount = -totalPrice,
                TransactionType = BillingTransactionType.OneTimeDebiting,
                UserId = buyParams.UserId,
                Description = "Web Hosting purchase"
            };
            var billingTransaction = this._billingService.AddUserTransaction(hostingBuyTransaction);

            var newHosting = this.PrepareNewHosting(buyParams, hostingTariff);

            var newHostingPayment = new WebHostingPayment
            {
                Amount = -totalPrice,
                BillingTransactionId = billingTransaction.Id,
                WebHostingId = newHosting.Id,
                Date = DateTime.UtcNow,
                MonthCount = buyParams.MonthCount
            };
            this._webHostingPaymentRepository.Add(newHostingPayment);
            this._unitOfWork.Commit();

            return newHosting;
        }

        private WebHosting PrepareNewHosting(BuyWebHostingParams buyParams, WebHostingTariff hostingTariff)
        {
            var task = new TaskV2
            {
                TypeTask = TypeTask.CreateWebHosting,
                UserId = buyParams.UserId
            };
            var options = new CreateWebHostingOptions();
            this._taskService.CreateTask(task, options);

            var hosting = new WebHosting
            {
                StorageSizeGB = hostingTariff.StorageSizeGB,
                UserId = buyParams.UserId,
                WebHostingTariffId = hostingTariff.Id,
                Status = WebHostingStatus.Creating,
                Name = buyParams.Name,
                AutoProlongation = buyParams.AutoProlongation,
                ExpireDate = DateTime.UtcNow.AddMonths(buyParams.MonthCount)
            };

            this._webHostingRepository.Add(hosting);
            this._unitOfWork.Commit();

            return hosting;
        }
    }
}
