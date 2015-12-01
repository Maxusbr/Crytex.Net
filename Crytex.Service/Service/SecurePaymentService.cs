using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models.Biling;
using Crytex.Service.IService.ISecureService;
using Microsoft.AspNet.Identity;
using System;
using System.Security.Principal;

namespace Crytex.Service.Service
{
    public class SecurePaymentService : PaymentService, ISecurePaymentService
    {
        private readonly IIdentity _userIdentity;

        public SecurePaymentService(IUnitOfWork unitOfWork, IBillingTransactionRepository billingRepo,
            IUserInfoRepository userInfoRepo, ICreditPaymentOrderRepository creditPaymentOrderRepo, IIdentity userIdentity) : 
                base(unitOfWork, billingRepo, userInfoRepo, creditPaymentOrderRepo)
        {
            this._userIdentity = userIdentity;
        }

        public override Payment GetCreditPaymentOrderById(Guid guid)
        {
            var order = base.GetCreditPaymentOrderById(guid);
            
            if(order.UserId != this._userIdentity.GetUserId())
            {
                throw new SecurityException($"Access denied for order with id={guid.ToString()}");
            }

            return order;
        }
    }
}
