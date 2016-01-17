using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.Biling
{
    public class UsageSubscriptionPayment : PaymentBase
    {
        public bool Paid { get; set; }
        public DateTime Date { get; set; }
        public int CoreCount { get; set; }
        public int RamCount { get; set; }
        public int HardDriveSize { get; set; }
        public decimal Amount { get; set; }
    }
}
