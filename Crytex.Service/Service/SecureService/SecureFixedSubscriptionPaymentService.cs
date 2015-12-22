using Crytex.Data.IRepository;
using Crytex.Service.IService;
using System.Security.Principal;
using Crytex.Model.Models.Biling;
using Crytex.Service.Model;
using PagedList;
using Microsoft.AspNet.Identity;
using Crytex.Model.Exceptions;

namespace Crytex.Service.Service.SecureService
{
    class SecureFixedSubscriptionPaymentService : FixedSubscriptionPaymentService, IFixedSubscriptionPaymentService
    {
        private readonly ISubscriptionVmRepository _subscriptionVmRepository;
        private readonly IIdentity _userIdentity;

        public SecureFixedSubscriptionPaymentService(IFixedSubscriptionPaymentRepository paymentRepo, ISubscriptionVmRepository subscriptionrepo,
            IIdentity userIdentity) : base(paymentRepo)
        {
            this._userIdentity = userIdentity;
            this._subscriptionVmRepository = subscriptionrepo;
        }

        public override IPagedList<FixedSubscriptionPayment> GetPage(int pageNumber, int pageSize, FixedSubscriptionPaymentSearchParams searchParams)
        {
            var userId = this._userIdentity.GetUserId();
            if (searchParams.SubscriptionVmId != null)
            {
                var sub = this._subscriptionVmRepository.GetById(searchParams.SubscriptionVmId.Value);
                if(sub.UserId != userId)
                {
                    throw new SecurityException($"Access denied for SubscriptionVm with id={searchParams.SubscriptionVmId}");
                }
            }
            searchParams.UserId = userId;

            return base.GetPage(pageNumber, pageSize, searchParams);
        }
    }
}
