using System;
using Crytex.Service.IService;
using Crytex.Model.Models.WebHostingModels;
using Crytex.Model.Models.Biling;
using Crytex.Data.IRepository;
using Crytex.Data.Infrastructure;
using Crytex.Model.Models;
using Crytex.Service.Model;
using PagedList;
using System.Linq.Expressions;
using Crytex.Service.Extension;
using Crytex.Model.Exceptions;
using System.Collections.Generic;

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

        public virtual WebHosting GetById(Guid webHostingId)
        {
            var hosting = this._webHostingRepository.Get(h => h.Id == webHostingId);

            if(hosting == null)
            {
                throw new InvalidIdentifierException($"WebHosting with id={webHostingId.ToString()} doesnt exist.");
            }

            return hosting;
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

        public void AutoProlongateWebHosting(Guid webHostingId, int monthCount)
        {
            try
            {
                this.ProlongateWebHosting(webHostingId, 1);
            }
            catch (TransactionFailedException)
            {
                this.UpdateWebHostingStatus(webHostingId, WebHostingStatus.WaitForPayment);
            }
        }

        public void ProlongateWebHosting(Guid webHostingId, int monthCount)
        {
            if(monthCount <= 0)
            {
                throw new ArgumentException("MonthCount parameter must be grater than 0");
            }

            var webHosting = this.GetById(webHostingId);
            var hostingTariff = this._webHostingTariffService.GetById(webHosting.WebHostingTariffId);
            var totalPrice = hostingTariff.Price * monthCount;
            var prolongateBillingTransaction = new BillingTransaction
            {
                CashAmount = -totalPrice,
                TransactionType = BillingTransactionType.OneTimeDebiting,
                UserId = webHosting.UserId,
                Description = "Web Hosting prolongation"
            };
            prolongateBillingTransaction = this._billingService.AddUserTransaction(prolongateBillingTransaction);

            var prolongateHostingPayment = new WebHostingPayment
            {
                Amount = -totalPrice,
                BillingTransactionId = prolongateBillingTransaction.Id,
                WebHostingId = webHosting.Id,
                Date = DateTime.UtcNow,
                MonthCount = monthCount
            };
            this._webHostingPaymentRepository.Add(prolongateHostingPayment);

            webHosting.ExpireDate = webHosting.ExpireDate.AddMonths(monthCount);
            webHosting.Status = WebHostingStatus.Active;
            this._webHostingRepository.Update(webHosting);
            this._unitOfWork.Commit();
        }

        public IPagedList<WebHostingPayment> GetWebHostingPaymentsPaged(int pageNumber, int pageSize, string userId = null,
            Guid? webHostingId = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("pageNumber and pageSize must be greater than zero");
            }

            var pageInfo = new PageInfo(pageNumber, pageSize);

            Expression<Func<WebHostingPayment, bool>> where = x => true;
            if(userId != null)
            {
                where = where.And(x => x.BillingTransaction.UserId == userId);
            }
            if(webHostingId != null)
            {
                where = where.And(x => x.WebHostingId == webHostingId.Value);
            }

            var page = this._webHostingPaymentRepository.GetPage(pageInfo, where, p => p.Date, false,
                p => p.WebHosting);

            return page;
        }

        public void UpdateWebHosting(Guid webHostingId, string name = null, bool? autoProlongation = null)
        {
            var hosting = this.GetById(webHostingId);

            if (name != null)
            {
                hosting.Name = name;
            }
            if (autoProlongation != null)
            {
                hosting.AutoProlongation = autoProlongation.Value;
            }

            this._webHostingRepository.Update(hosting);
            this._unitOfWork.Commit();
        }

        public IEnumerable<WebHosting> GetAllByStatusAndExpireDate(WebHostingStatus status, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            Expression<Func<WebHosting, bool>> where = h => h.Status == status;
            if(dateFrom != null)
            {
                where = where.And(h => h.ExpireDate >= dateFrom);
            }
            if (dateTo != null)
            {
                where = where.And(h => h.ExpireDate < dateTo);
            }

            var hostings = this._webHostingRepository.GetMany(where);

            return hostings;
        }

        public void UpdateWebHostingStatus(Guid id, WebHostingStatus newStatus)
        {
            var hosting = this.GetById(id);
            hosting.Status = newStatus;

            this._webHostingRepository.Update(hosting);
            this._unitOfWork.Commit();
        }

        public void PrepareHostingForDeletion(Guid id)
        {
            var hosting = this.GetById(id);
            // create web-hosting disable task
            var disableHostingOptions = new DisableWebHostingOptions
            {
            };
            var task = new TaskV2
            {
                UserId = hosting.UserId,
                TypeTask = TypeTask.DisableWebHosting
            };
            this._taskService.CreateTask(task, disableHostingOptions);

            // Change subscription status to WaitForDeletion
            hosting.Status = WebHostingStatus.WaitForDeletion;
            this._webHostingRepository.Update(hosting);
            this._unitOfWork.Commit();
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
