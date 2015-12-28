using System.Collections.Generic;

namespace Crytex.Web.Models.JsonModels
{
    public class UsageSubscriptionPaymentGroupByVmView
    {
        public string Name { get; set; }
        public IEnumerable<UsageSubscriptionPaymentByPeriodView> Subscriptions { get; set; }
    }
}