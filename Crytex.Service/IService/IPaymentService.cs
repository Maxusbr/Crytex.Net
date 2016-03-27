using PagedList;
using System;
using Crytex.Model.Models.Biling;
using Crytex.Service.Model;

namespace Crytex.Service.IService
{
    public interface IPaymentService
    {
        Payment CreateCreditPaymentOrder(decimal cashAmount, string userId, Guid paymentSystem);
        void ConfirmCreditPaymentOrder(Guid id, decimal cashAmount);
        void FailCreditPaymentOrder(Guid id);

        void DeleteCreditPaymentOrderById(Guid id);

        Payment GetCreditPaymentOrderById(Guid guid);

        IPagedList<Payment> GetPage(int pageNumber, int pageSize, SearchPaymentParams filter = null);

        IPagedList<BillingTransactionInfo> GetUserBillingTransactionInfosPage(string userId, int pageNumber, int pageSize, DateTime? from = null, DateTime? to = null);
    }
}
