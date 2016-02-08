using System;

namespace Crytex.Model.Models.Biling
{
    public class SubscriptionVmBackupPayment : SubscriptionPaymentBase
    {
        public int DaysPeriod { get; set; }
        public bool Paid { get; set; }
        public DateTime DateEnd { get; set; }
        public DateTime DateStart { get; set; }
    }
}
