using System;
using System.Collections.Generic;
using Crytex.Model.Enums;

namespace Crytex.Web.Models.JsonModels
{
    public class UsageSubscriptionPaymentByPeriodView
    {
        public string Month { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<UsageSubscriptionPaymentView> UsageSubscriptionPayment { get; set; }
    }
}