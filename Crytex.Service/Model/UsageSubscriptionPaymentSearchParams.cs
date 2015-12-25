using System;
using Crytex.Model.Enums;
using Crytex.Model.Models;

namespace Crytex.Service.Model
{
    public class UsageSubscriptionPaymentSearchParams
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Guid? SubscriptionVmId { get; set; }
        public TypeVirtualization? TypeVirtualization { get; set; }
        public CountingPeriodType? PeriodType { get; set; }
    }
}
