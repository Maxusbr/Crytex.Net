﻿using System;
using Quartz;
using Crytex.Service.IService;
using Crytex.Model.Models.Biling;
using System.Linq;
using Crytex.Service.Model;

namespace Crytex.Background.Tasks.SubscriptionVm
{
    [DisallowConcurrentExecution]
    public class ActiveStaticSubscriptionVmJob : IJob
    {
        private readonly IBilingService _billingService;
        private readonly ISubscriptionVmService _subscriptionService;

        public ActiveStaticSubscriptionVmJob(ISubscriptionVmService subscriptionService, IBilingService billingService)
        {
            this._subscriptionService = subscriptionService;
            this._billingService = billingService;
        }

        public void Execute(IJobExecutionContext context)
        {
            var subscriptions = this._subscriptionService.GetSubscriptionsByStatusAndType(SubscriptionVmStatus.Active, SubscriptionType.Fixed);
            var currentDate = DateTime.UtcNow;

            var outdatedSubs = subscriptions.Where(s => s.DateEnd < currentDate);

            foreach(var sub in outdatedSubs)
            {
                if (sub.AutoProlongation)
                {
                    this._subscriptionService.AutoProlongateSubscription(sub.Id);
                }
                else
                {
                    this._subscriptionService.UpdateSubscriptionStatus(sub.Id, SubscriptionVmStatus.WaitForPayment);
                }
            }
        }
    }
}
