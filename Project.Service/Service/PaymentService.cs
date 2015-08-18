using PagedList;
using Project.Data.Infrastructure;
using Project.Data.IRepository;
using Project.Model.Exceptions;
using Project.Model.Models;
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
        private ICreditPaymentOrderRepository _creditPaymentOrderRepository;

        public PaymentService(IUnitOfWork unitOfWork, IBillingTransactionRepository billingRepo,
            IUserInfoRepository userInfoRepo, ICreditPaymentOrderRepository creditPaymentOrderRepo)
        {
            this._unitOfWork = unitOfWork;
            this._billingTransactionRepository = billingRepo;
            this._userInfoRepository = userInfoRepo;
            this._creditPaymentOrderRepository = creditPaymentOrderRepo;
        }

        public CreditPaymentOrder CreateCreditPaymentOrder(decimal cashAmount, string userId, PaymentSystemType paymentSystem)
        {
            var newOrder = new CreditPaymentOrder
            {
                CashAmount = cashAmount,
                UserId = userId,
                PaymentSystem = paymentSystem,
                Date = DateTime.UtcNow
            };

            this._creditPaymentOrderRepository.Add(newOrder);
            this._unitOfWork.Commit();

            return newOrder;
        }


        public void DeleteCreditPaymentOrderById(Guid id)
        {
            var orderToDelete = this._creditPaymentOrderRepository.GetById(id);
            if (orderToDelete == null)
            {
                throw new InvalidIdentifierException(string.Format("CreditPaymentOrder with id = {0} doesn't exist.", id.ToString()));
            }

            this._creditPaymentOrderRepository.Delete(orderToDelete);
            this._unitOfWork.Commit();
        }


        public CreditPaymentOrder GetCreditPaymentOrderById(Guid guid)
        {
            var order = this._creditPaymentOrderRepository.GetById(guid);
            if (order == null)
            {
                throw new InvalidIdentifierException(string.Format("CreditPaymentOrder with id = {0} doesn't exist.", guid.ToString()));
            }

            return order;
        }


        public IPagedList<CreditPaymentOrder> GetPage(int pageNumber, int pageSize)
        {
            var page = this._creditPaymentOrderRepository.GetPage(new Page(pageNumber, pageSize), (x => true), (x => x.Date));

            return page;
        }
    }
}
