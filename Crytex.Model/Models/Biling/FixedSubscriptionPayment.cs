using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.Biling
{
    public class FixedSubscriptionPayment : GuidBaseEntity
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int MonthCount { get; set; }
        public int CoreCount { get; set; }
        public int RamCount { get; set; }
        public int HardDriveSize { get; set; }
        public int TariffId { get; set; }
        public DateTime DateEnd { get; set; }
        public DateTime DateStart { get; set; }
        public Guid SubscriptionVmId { get; set; }
        public Guid BillingTransactionId { get; set; }

        [ForeignKey("TariffId")]
        public Tariff Tariff { get; set; }
        [ForeignKey("SubscriptionVmId")]
        public SubscriptionVm SubscriptionVm { get; set; }
        [ForeignKey("BillingTransactionId")]
        public BillingTransaction BillingTransaction { get; set; }
    }
}
