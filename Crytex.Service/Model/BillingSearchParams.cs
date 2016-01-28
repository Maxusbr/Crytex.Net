using System;

namespace Crytex.Service.Model
{
    public class BillingSearchParams
    {
        public string UserId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? BillingTransactionType { get; set; }
        public Guid? SubscriptionVmId { get; set; }
        public Guid? SubscriptionGameServerId { get; set; }
        public int? SubscriptionVmMonthCount { get; set; }
    }
}
