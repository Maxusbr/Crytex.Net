using System;
using System.Collections.Generic;
using Crytex.Model.Models.Biling;

namespace Crytex.Service.Model
{
    public class UsageSubscriptionPaymentContainer
    {
        public string Month { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<UsageSubscriptionPayment> UsageSubscriptionPayment { get; set; }
    }
}
