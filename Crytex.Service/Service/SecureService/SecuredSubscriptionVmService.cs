using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Service.IService;
using System.Security.Principal;
using Crytex.Model.Models.Biling;
using System;
using Microsoft.AspNet.Identity;
using Crytex.Model.Exceptions;

namespace Crytex.Service.Service.SecureService
{
    public class SecuredSubscriptionVmService : SubscriptionVmService, ISubscriptionVmService
    {
        private readonly IIdentity _userIdentity;

        public SecuredSubscriptionVmService(ISubscriptionVmRepository subVmRepo, IUnitOfWork unitOfWork, ITaskV2Service taskService,
            IBilingService billingService, ITariffInfoService tariffInfoService, IOperatingSystemsService operatingSystemService,
            IIdentity userIdentity) : base(unitOfWork, subVmRepo, taskService, billingService, tariffInfoService, operatingSystemService)
        {
            this._userIdentity = userIdentity;
        }

        public override SubscriptionVm GetById(Guid guid)
        {
            var sub = base.GetById(guid);

            if(sub.UserId != this._userIdentity.GetUserId())
            {
                throw new SecurityException($"Access denied for vm subscription with id={guid.ToString()}");
            }

            return sub;
        }
    }
}
