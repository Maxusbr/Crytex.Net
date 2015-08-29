using PagedList;
using Crytex.Model.Models;
using System;

namespace Crytex.Service.IService
{
    public interface IPaymentService
    {
        CreditPaymentOrder CreateCreditPaymentOrder(decimal cashAmount, string userId, PaymentSystemType paymentSystem);

        void DeleteCreditPaymentOrderById(Guid id);

        CreditPaymentOrder GetCreditPaymentOrderById(Guid guid);

        IPagedList<CreditPaymentOrder> GetPage(int pageNumber, int pageSize);
    }
}
