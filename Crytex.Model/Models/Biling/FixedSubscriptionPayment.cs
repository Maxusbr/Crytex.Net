using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models.Biling
{
    public class FixedSubscriptionPayment : SubscriptionPaymentBase
    {
        public int? MonthCount { get; set; }
        public int CoreCount { get; set; }
        public int RamCount { get; set; }
        public int HardDriveSize { get; set; }
        public DateTime DateEnd { get; set; }
        public DateTime DateStart { get; set; }
    }
}
