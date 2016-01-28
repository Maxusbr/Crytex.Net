using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Service.IService;
using System.Security.Principal;
using Crytex.Model.Models.WebHostingModels;
using System;
using Microsoft.AspNet.Identity;
using Crytex.Model.Exceptions;

namespace Crytex.Service.Service.SecureService
{
    class SecureWebHostingService : WebHostingService, IWebHostingService
    {
        private readonly IIdentity _userIdentity;

        public SecureWebHostingService(IWebHostingTariffService webHostingTariffService, IBilingService billingService,
            IWebHostingRepository webHostingRepository, ITaskV2Service taskService, IWebHostingPaymentRepository webHostingPaymentRepo,
            IUnitOfWork unitOfWork, IIdentity identity)
            : base(webHostingTariffService, billingService, webHostingRepository, taskService, webHostingPaymentRepo, unitOfWork)
        {
            this._userIdentity = _userIdentity;
        }

        public override WebHosting GetById(Guid webHostingId)
        {
            var hosting = base.GetById(webHostingId);
            if (hosting.UserId != this._userIdentity.GetUserId())
            {
                throw new SecurityException($"Access for WebHosting with id={webHostingId.ToString()} is denied");
            }

            return hosting;
        }
    }
}
