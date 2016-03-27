using System;
using Crytex.Model.Enums;
using Crytex.Model.Models;

namespace Crytex.Service.Model
{
    public class UsageSubscriptionPaymentSearchParams
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public Guid? SubscriptionVmId { get; set; }
        public TypeVirtualization? TypeVirtualization { get; set; }
        public CountingPeriodType? PeriodType { get; set; }
        public UsageSubscriptionPaymentGroupingTypes? GroupingType { get; set; }
    }
}
