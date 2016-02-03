using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.Biling
{
    public abstract class SubscriptionPaymentBase : PaymentBase
    {
        public int TariffId { get; set; }
        public Guid SubscriptionVmId { get; set; }


        [ForeignKey("TariffId")]
        public Tariff Tariff { get; set; }
        [ForeignKey("SubscriptionVmId")]
        public SubscriptionVm SubscriptionVm { get; set; }
    }
}
