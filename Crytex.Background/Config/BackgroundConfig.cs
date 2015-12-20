using System;
using Crytex.Core.AppConfig;

namespace Crytex.Background.Config
{
    public class BackgroundConfig : AppConfig, IBackgroundConfig
    {
        public int GetSubscriptionVmEndWarnPeriod()
        {
            return this.GetValue<int>("SubscriptionVmEndWarnPeriod");
        }

        public int GetSubscriptionVmWaitForDeletionActionPeriod()
        {
            return this.GetValue<int>("SubscriptionVmWaitForDeletionActionPeriod");
        }

        public int GetSubscriptionVmWaitForPaymentActionPeriod()
        {
            return this.GetValue<int>("SubscriptionVmWaitForPaymentActionPeriod");
        }
    }
}
