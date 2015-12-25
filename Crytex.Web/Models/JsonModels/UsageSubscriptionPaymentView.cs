using System;

namespace Crytex.Web.Models.JsonModels
{
    public class UsageSubscriptionPaymentView
    {
        public Guid Id { get; set; }
        public Guid SubscriptionVmId { get; set; }
        public int TariffId { get; set; }
        public string UserId { get; set; }
        public Guid UserVmId { get; set; }
        public string UserVmName { get; set; }
        public string UserName { get; set; }
        public bool Paid { get; set; }
        public DateTime Date { get; set; }
        public int CoreCount { get; set; }
        public int RamCount { get; set; }
        public int HardDriveSize { get; set; }
        public decimal Amount { get; set; }
    }
}