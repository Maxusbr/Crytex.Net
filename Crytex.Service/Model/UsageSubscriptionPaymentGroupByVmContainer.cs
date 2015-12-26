using System.Collections.Generic;
using Crytex.Model.Models.Biling;

namespace Crytex.Service.Model
{
    public class UsageSubscriptionPaymentGroupByVmContainer
    {
        public string Name { get; set; }
        public IEnumerable<UsageSubscriptionPaymentContainer> Subscriptions { get; set; }
    }
}
