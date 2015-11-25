using PagedList;
using Crytex.Model.Models;
using System;
using Crytex.Model.Models.Biling;
using Crytex.Service.Model;

namespace Crytex.Service.IService
{
    public interface IPaymentService
    {
        Payment CreateCreditPaymentOrder(decimal cashAmount, string userId, PaymentSystemType paymentSystem);

        void DeleteCreditPaymentOrderById(Guid id);

        Payment GetCreditPaymentOrderById(Guid guid);

        IPagedList<Payment> GetPage(int pageNumber, int pageSize, SearchPaymentParams filter = null);
    }
}
