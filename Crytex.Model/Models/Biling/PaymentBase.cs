using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.Biling
{
    public abstract class PaymentBase : GuidBaseEntity
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int TariffId { get; set; }
        public Guid SubscriptionVmId { get; set; }
        public Guid? BillingTransactionId { get; set; }


        [ForeignKey("TariffId")]
        public Tariff Tariff { get; set; }
        [ForeignKey("SubscriptionVmId")]
        public SubscriptionVm SubscriptionVm { get; set; }
        [ForeignKey("BillingTransactionId")]
        public BillingTransaction BillingTransaction { get; set; }
    }
}
