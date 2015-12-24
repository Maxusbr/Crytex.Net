using System;
using Crytex.Model.Models;

namespace Crytex.Web.Models.JsonModels
{
    public class FixedSubscriptionPaymentSearchParamViewModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string SubscriptionVmId { get; set; }
    }

    public class AdminFixedSubscriptionPaymentSearchParamViewModel : FixedSubscriptionPaymentSearchParamViewModel
    {        
        public string UserId { get; set; }
        public TypeVirtualization? Virtualization { get; set; }
        public OperatingSystemFamily? OperatingSystem { get; set; }
    }
}