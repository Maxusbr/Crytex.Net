using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crytex.Model.Models.Biling;

namespace Crytex.Web.Models.JsonModels
{
    public class BillingSearchParamsViewModel
    {
        public string UserId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public BillingTransactionType? BillingTransactionType { get; set; }
    }

    public class AdminBillingSearchParamsViewModel : BillingSearchParamsViewModel
    {
        public Guid? SubscriptionVmId { get; set; }
        public Guid? SubscriptionGameServerId { get; set; }
    }
}