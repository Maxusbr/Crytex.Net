using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.Biling
{
    public class UsageSubscriptionPayment : GuidBaseEntity
    {
        public Guid SubscriptionVmId { get; set; }
        public Guid? BillingTransactionId { get; set; }
        public bool Paid { get; set; }
        public DateTime Date { get; set; }
        public int CoreCount { get; set; }
        public int RamCount { get; set; }
        public int HardDriveSize { get; set; }
        public int TariffId { get; set; }
        public decimal Amount { get; set; }

        [ForeignKey("SubscriptionVmId")]
        public SubscriptionVm SubscriptionVm { get; set; }
        [ForeignKey("BillingTransactionId")]
        public BillingTransaction BillingTransaction { get; set; }
        [ForeignKey("TariffId")]
        public Tariff Tariff { get; set; }
    }
}
