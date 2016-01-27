using System;
using Crytex.Service.IService;
using Crytex.Model.Models.WebHosting;
using Crytex.Model.Models.Biling;
using Crytex.Data.IRepository;
using Crytex.Data.Infrastructure;
using Crytex.Model.Models;

namespace Crytex.Service.Service
{
    class WebHostingService : IWebHostingService
    {
        private readonly IBilingService _billingService;
        private readonly ITaskV2Service _taskService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostingRepository _webHostingRepository;
        private readonly IWebHostingTariffService _webHostingTariffService;

        public WebHostingService(IWebHostingTariffService webHostingTariffService, IBilingService billingService,
            IWebHostingRepository webHostingRepository, ITaskV2Service taskService, IUnitOfWork unitOfWork)
        {
            this._webHostingTariffService = webHostingTariffService;
            this._billingService = billingService;
            this._webHostingRepository = webHostingRepository;
            this._taskService = taskService;
            this._unitOfWork = unitOfWork;
        }

        public WebHosting BuyNewHosting(Guid hostingTariffId, string userId)
        {
            var hostingTariff = this._webHostingTariffService.GetById(hostingTariffId);
            var hostingBuyTransaction = new BillingTransaction
            {
                CashAmount = -hostingTariff.Price,
                TransactionType = BillingTransactionType.OneTimeDebiting,
                UserId = userId,
                Description = "Web Hosting purchase"
            };
            this._billingService.AddUserTransaction(hostingBuyTransaction);

            var newHosting = this.PrepareNewHosting(hostingTariff, userId);

            return newHosting;
        }

        private WebHosting PrepareNewHosting(WebHostingTariff hostingTariff, string userId)
        {
            var task = new TaskV2
            {
                TypeTask = TypeTask.CreateWebHosting,
                UserId = userId
            };
            var options = new CreateWebHostingOptions();
            this._taskService.CreateTask(task, options);

            var hosting = new WebHosting
            {
                StorageSizeGB = hostingTariff.StorageSizeGB,
                UserId = userId,
                WebHostingTariffId = hostingTariff.Id,
                Status = WebHostingStatus.Creating
            };

            this._webHostingRepository.Add(hosting);
            this._unitOfWork.Commit();

            return hosting;
        }
    }
}
