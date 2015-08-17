using Project.Data.Infrastructure;
using Project.Data.IRepository;
using Project.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Service
{
    public class PaymentService : IPaymentService
    {
        private IUnitOfWork _unitOfWork;
        private IBillingTransactionRepository _billingTransactionRepository;
        private IUserInfoRepository _userInfoRepository;

        public PaymentService(IUnitOfWork unitOfWork, IBillingTransactionRepository billingRepo, IUserInfoRepository userInfoRepo)
        {
            this._unitOfWork = unitOfWork;
            this._billingTransactionRepository = billingRepo;
            this._userInfoRepository = userInfoRepo;
        }
    }
}
