using System;
using Quartz;
using Crytex.Service.IService;
using Crytex.Background.Config;
using Crytex.Notification;
using Crytex.Model.Models.WebHostingModels;

namespace Crytex.Background.Tasks.WebHosting
{
    [DisallowConcurrentExecution]
    public class WebHostingWarningsJob : IJob
    {
        private readonly IBackgroundConfig _config;
        private readonly INotificationManager _notificationManager;
        private readonly IWebHostingService _webHostingService;

        public WebHostingWarningsJob(IWebHostingService webHostingService, IBackgroundConfig config, INotificationManager notificationManager)
        {
            this._webHostingService = webHostingService;
            this._config = config;
            this._notificationManager = notificationManager;
        }


        public void Execute(IJobExecutionContext context)
        {
            var currentDate = DateTime.UtcNow;
            var hostingEndWarnPeriod = this._config.GetWebHostingEndWarnPeriod();
            var warningDateFrom = currentDate.AddDays(hostingEndWarnPeriod);
            var warningDateTo = currentDate.AddDays(hostingEndWarnPeriod + 1).AddMinutes(-1);
            var hostingsExpiredSoon = this._webHostingService.GetAllByStatusAndExpireDate(WebHostingStatus.Active, warningDateFrom, warningDateTo);

            foreach(var hosting in hostingsExpiredSoon)
            {
                var daysToEnd = (hosting.ExpireDate - currentDate).Days;
                if (daysToEnd == hostingEndWarnPeriod)
                {
                    this._notificationManager.SendWebHostingEndWarningEmail(hosting.UserId, hostingEndWarnPeriod);
                }
            }

            var waitForDeletionHostings = this._webHostingService.GetAllByStatusAndExpireDate(WebHostingStatus.WaitForDeletion);
            var deletionPeriod = this._config.GetWebHostingWaitForDeletionActionPeriod();
            foreach(var hosting in waitForDeletionHostings)
            {
                var daysToDeletion = (hosting.ExpireDate.AddDays(deletionPeriod) - currentDate).Days;
                this._notificationManager.SendSubscriptionDeletionWarningEmail(hosting.UserId, daysToDeletion);
            }
        }
    }
}
