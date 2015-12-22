using System;
using Quartz;
using Crytex.Service.IService;
using Crytex.Model.Models.Biling;
using Crytex.Background.Config;
using System.Linq;
using Crytex.Notification;

namespace Crytex.Background.Tasks.SubscriptionVm
{
    [DisallowConcurrentExecution]
    public class WaitFotPaymentStaticSubscriptionVmJob : IJob
    {
        private readonly IBackgroundConfig _config;
        private readonly INotificationManager _notificationManager;
        private readonly ISubscriptionVmService _subscriptionService;
        private readonly ITaskV2Service _taskService;

        public WaitFotPaymentStaticSubscriptionVmJob(ISubscriptionVmService subscriptionService, ITaskV2Service taskService,
            IBackgroundConfig config, INotificationManager notificationManager)
        {
            this._subscriptionService = subscriptionService;
            this._taskService = taskService;
            this._config = config;
            this._notificationManager = notificationManager;
        }

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Wait for payment sub job");
            var subs = this._subscriptionService.GetSubscriptionsByStatusAndType(SubscriptionVmStatus.WaitForPayment, SubscriptionType.Fixed);
            var currentDate = DateTime.UtcNow;
            var emailPeriod = this._config.GetSubscriptionVmWaitForPaymentActionPeriod();

            var actionRequiredSubs = subs.Where(sub => (currentDate - sub.DateEnd).Days >= emailPeriod);
            foreach(var sub in actionRequiredSubs)
            {
                this._subscriptionService.PrepareSubscriptionForDeletion(sub.Id);
                this._notificationManager.SendMachinePoweredOffEmail(sub.UserId);
            }
        }
    }
}
