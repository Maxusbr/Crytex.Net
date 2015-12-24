using System;
using Crytex.Model.Models;

namespace Crytex.Service.Model
{
    public class FixedSubscriptionPaymentSearchParams
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string UserId { get; set; }
        public string SubscriptionVmId { get; set; }
        public TypeVirtualization? Virtualization { get; set; }
        public OperatingSystemFamily? OperatingSystem { get; set; }
    }
}
