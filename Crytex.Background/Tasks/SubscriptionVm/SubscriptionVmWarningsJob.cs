using Quartz;
using Crytex.Service.IService;
using Crytex.Background.Config;
using System;
using Crytex.Notification;
using Crytex.Model.Models.Biling;

namespace Crytex.Background.Tasks.SubscriptionVm
{
    public class SubscriptionVmWarningsJob : IJob
    {
        private readonly IBackgroundConfig _config;
        private readonly INotificationManager _notificationManager;
        private readonly ISubscriptionVmService _subscriptionService;

        public SubscriptionVmWarningsJob(ISubscriptionVmService subscriptionService, IBackgroundConfig config, 
            INotificationManager notificationManager)
        {
            this._subscriptionService = subscriptionService;
            this._config = config;
            this._notificationManager = notificationManager;
        }

        public void Execute(IJobExecutionContext context)
        {
            var subs = this._subscriptionService.GetAllSubscriptions();
            var subscriptionEndWarnPeriod = this._config.GetSubscriptionVmEndWarnPeriod();
            var deletionPeriod = this._config.GetSubscriptionVmWaitForDeletionActionPeriod();
            var currentDate = DateTime.UtcNow;

            foreach(var sub in subs)
            {
                var daysToEnd = (currentDate - sub.DateEnd).Days;
                if (currentDate > sub.DateEnd &&  daysToEnd == subscriptionEndWarnPeriod)
                {
                    this._notificationManager.SendSubscriptionEndWarningEmail(sub.UserId, subscriptionEndWarnPeriod);
                }
                if(sub.Status == SubscriptionVmStatus.WaitForDeletion)
                {
                    var daysToDeletion = (sub.DateEnd.AddDays(deletionPeriod) - currentDate).Days;
                    this._notificationManager.SendSubscriptionDeletionWarningEmail(sub.UserId, daysToDeletion);
                }
            }
        }
    }
}
