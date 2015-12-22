using Quartz;
using Crytex.Service.IService;
using System;
using Crytex.Model.Models.Biling;

namespace Crytex.Background.Tasks.SubscriptionVm
{
    [DisallowConcurrentExecution]
    public class UsageSubscriptionVmJob : IJob
    {
        private readonly ISubscriptionVmService _subscriptionService;

        public UsageSubscriptionVmJob(ISubscriptionVmService subscriptionService)
        {
            this._subscriptionService = subscriptionService;
        }

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("UsageSubscriptionVmJob");
            var activeUsageSubs = this._subscriptionService.GetSubscriptionsByStatusAndType(SubscriptionVmStatus.Active, SubscriptionType.Usage);
            
            foreach(var sub in activeUsageSubs)
            {
                this._subscriptionService.UpdateUsageSubscriptionBalance(sub.Id);
            }
        }
    }
}
