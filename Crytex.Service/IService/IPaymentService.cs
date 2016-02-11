using PagedList;
using Crytex.Model.Models;
using System;
using System.Collections.Generic;
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

        void EnableDisablePaymentSystem(Guid id, bool enable);
        IEnumerable<PaymentSystem> GetPaymentSystems(bool searchEnabled = false);

        IEnumerable<BillingTransactionInfo> GetUserBillingTransactionInfos(string userId);
    }
}
