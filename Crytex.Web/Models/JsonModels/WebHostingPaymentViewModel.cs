using System;

namespace Crytex.Web.Models.JsonModels
{
    public class WebHostingPaymentViewModel : PaymentViewModelBase
    {
        public string WebHostingName { get; set; }
        public Guid WebHostingId { get; set; }

        public override PaymentViewModelType PaymentModelType
        {
            get
            {
                return PaymentViewModelType.WebHosting;
            }
        }
    }
}