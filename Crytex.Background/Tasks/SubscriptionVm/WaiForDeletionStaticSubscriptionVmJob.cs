using Quartz;
using Crytex.Service.IService;
using System;
using Crytex.Model.Models.Biling;
using Crytex.Background.Config;
using System.Linq;

namespace Crytex.Background.Tasks.SubscriptionVm
{
    [DisallowConcurrentExecution]
    public class WaiForDeletionStaticSubscriptionVmJob : IJob
    {
        private readonly IBackgroundConfig _config;
        private readonly ISubscriptionVmService _subscriptionService;

        public WaiForDeletionStaticSubscriptionVmJob(ISubscriptionVmService subscriptionService, IBackgroundConfig config)
        {
            this._subscriptionService = subscriptionService;
            this._config = config;
        }

        public void Execute(IJobExecutionContext context)
        {
            var subs = this._subscriptionService.GetSubscriptionsByStatusAndType(SubscriptionVmStatus.WaitForDeletion, SubscriptionType.Fixed);
            var emailPeriod = this._config.GetSubscriptionVmWaitForDeletionActionPeriod();
            var currentDate = DateTime.UtcNow;
            var actionRequiredSubs = subs.Where(sub => (currentDate - sub.DateEnd).Days >= emailPeriod);

            foreach(var sub in actionRequiredSubs)
            {
                this._subscriptionService.DeleteSubscription(sub.Id);
            }
        }
    }
}
