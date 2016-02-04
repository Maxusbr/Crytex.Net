using PagedList;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Crytex.Model.Models.Biling;
using Crytex.Service.Extension;
using Crytex.Service.Model;

namespace Crytex.Service.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IBillingTransactionRepository _billingTransactionRepository;
        private readonly ICreditPaymentOrderRepository _creditPaymentOrderRepository;
        private readonly IPaymentSystemRepository _paymentSystemRepository;

        public PaymentService(IUnitOfWork unitOfWork, IBillingTransactionRepository billingRepo,
            ICreditPaymentOrderRepository creditPaymentOrderRepo, IPaymentSystemRepository paymentSystemRepository)
        {
            this._unitOfWork = unitOfWork;
            this._billingTransactionRepository = billingRepo;
            this._creditPaymentOrderRepository = creditPaymentOrderRepo;
            _paymentSystemRepository = paymentSystemRepository;
        }

        public Payment CreateCreditPaymentOrder(decimal cashAmount, string userId, Guid paymentSystem)
        {
            var psystem = _paymentSystemRepository.GetById(paymentSystem);
            if (psystem == null)
            {
                throw new InvalidIdentifierException($"Payment System with id = {paymentSystem} doesn't exist.");
            }
            if (!psystem.IsEnabled)
            {
                throw new InvalidIdentifierException($"Payment System with id = {paymentSystem} is disable.");
            }
            var newOrder = new Payment
            {
                Status = PaymentStatus.Created,
                CashAmount = cashAmount,
                UserId = userId,
                PaymentSystemId = paymentSystem,
                Date = DateTime.UtcNow
            };

            _creditPaymentOrderRepository.Add(newOrder);
            _unitOfWork.Commit();

            return newOrder;
        }

        public void ConfirmCreditPaymentOrder(Guid id, decimal cashAmount)
        {
            var payment = _creditPaymentOrderRepository.GetById(id);
            if (payment == null)
            {
                throw new InvalidIdentifierException($"CreditPaymentOrder with id = {id} doesn't exist.");
            }
            payment.AmountReal = cashAmount;
            payment.Status = PaymentStatus.Success;
            //TODO Add transaction?
            _creditPaymentOrderRepository.Update(payment);
            _unitOfWork.Commit();
        }

        public void FailCreditPaymentOrder(Guid id)
        {
            var payment = _creditPaymentOrderRepository.GetById(id);
            if (payment == null)
            {
                throw new InvalidIdentifierException($"CreditPaymentOrder with id = {id} doesn't exist.");
            }
            payment.Status = PaymentStatus.Failed;
            _creditPaymentOrderRepository.Update(payment);
            _unitOfWork.Commit();
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

                if (filter.Status != null)
                {
                    where = where.And(p => p.Status == filter.Status);
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
                if (filter.PaymentSystemId != null)
                {
                    where = where.And(x => x.PaymentSystemId == filter.PaymentSystemId);
                }
            }

            var page = this._creditPaymentOrderRepository.GetPage(new PageInfo(pageNumber, pageSize), where, (x => x.Date), true,
                x => x.User, x => x.PaymentSystem);

            return page;
        }

        public void EnableDisablePaymentSystem(Guid id, bool enable)
        {
            var paymentSystem = _paymentSystemRepository.GetById(id);
            if (paymentSystem == null)
            {
                throw new InvalidIdentifierException($"Payment System with id = {id} doesn't exist.");
            }
            paymentSystem.IsEnabled = enable;
            _paymentSystemRepository.Update(paymentSystem);
            _unitOfWork.Commit();
        }

        public IEnumerable<PaymentSystem> GetPaymentSystems(bool searchEnabled = false)
        {
            Expression<Func<PaymentSystem, bool>> where = x => true;
            if(searchEnabled)
                where = where.And(x => x.IsEnabled);
            var list = _paymentSystemRepository.GetMany(where);
            return list;
        }
    }
}
