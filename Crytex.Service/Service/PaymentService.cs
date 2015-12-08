using PagedList;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.IService;
using System;
using System.Linq.Expressions;
using Crytex.Model.Models.Biling;
using Crytex.Service.Extension;
using Crytex.Service.Model;

namespace Crytex.Service.Service
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

        public Payment CreateCreditPaymentOrder(decimal cashAmount, string userId, PaymentSystemType paymentSystem)
        {
            var newOrder = new Payment
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
            var orderToDelete = this.GetCreditPaymentOrderById(id);

            this._creditPaymentOrderRepository.Delete(orderToDelete);
            this._unitOfWork.Commit();
        }


        public virtual Payment GetCreditPaymentOrderById(Guid guid)
        {
            var order = this._creditPaymentOrderRepository.GetById(guid);
            if (order == null)
            {
                throw new InvalidIdentifierException(string.Format("CreditPaymentOrder with id = {0} doesn't exist.", guid.ToString()));
            }

            return order;
        }


        public IPagedList<Payment> GetPage(int pageNumber, int pageSize, SearchPaymentParams filter = null)
        {
            Expression<Func<Payment, bool>> where = x => true;

            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.UserId))
                {
                    where = where.And(p => p.UserId == filter.UserId);
                }

                if (filter.Success != null)
                {
                    where = where.And(p => p.Success == filter.Success);
                }

                if (filter.DateType != null)
                {

                    if (filter.DateType == DateType.StartTransaction)
                    {
                        where = where.And(x => x.Date >= filter.FromDate && x.Date <= filter.ToDate);
                    }
                    if (filter.DateType == DateType.EndTramsaction)
                    {
                        where = where.And(x => x.DateEnd >= filter.FromDate && x.DateEnd <= filter.ToDate);
                    }
                }
                if (filter.PaymentSystem != null)
                {
                    where = where.And(x => x.PaymentSystem == filter.PaymentSystem);
                }
            }

            var page = this._creditPaymentOrderRepository.GetPage(new PageInfo(pageNumber, pageSize), where, (x => x.Date), true,x=>x.User);

            return page;
        }



    }
}
