using System;
using System.Collections.Generic;
using Crytex.Model.Models.Biling;

namespace Crytex.Service.IService
{
    public interface IPaymentSystemService
    {
        IEnumerable<PaymentSystem> GetPaymentSystems(Boolean searchEnabled = false);

        PaymentSystem GetPaymentSystemById(String id);

        PaymentSystem Create(PaymentSystem paymentSystem);

        void Update(PaymentSystem paymentSystem);

        void Delete(String id);
    }
}
