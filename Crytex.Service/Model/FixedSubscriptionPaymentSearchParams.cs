using System;

namespace Crytex.Service.Model
{
    public class FixedSubscriptionPaymentSearchParams
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public Guid? SubscriptionVmId { get; set; }
        public string UserId { get; set; }
    }
}
