using System;
using Quartz;
using Crytex.Service.IService;
using Crytex.Background.Config;
using Crytex.Model.Models.WebHostingModels;
using Crytex.Notification;

namespace Crytex.Background.Tasks.WebHosting
{
    [DisallowConcurrentExecution]
    public class WaitForPaymentWebHostingJob : IJob
    {
        private readonly IBackgroundConfig _config;
        private readonly INotificationManager _notificationManager;
        private readonly IWebHostingService _webHostingService;

        public WaitForPaymentWebHostingJob(IWebHostingService webHostingService, IBackgroundConfig config, INotificationManager notificationManager)
        {
            this._webHostingService = webHostingService;
            this._config = config;
            this._notificationManager = notificationManager;
        }
        public void Execute(IJobExecutionContext context)
        {
            var emailPeriod = this._config.GetWebHostingWaitForPaymentActionPeriod();
            var currentDate = DateTime.UtcNow;
            var dateExpireTo = currentDate.AddDays(-emailPeriod);
            var hostings = this._webHostingService.GetAllByStatusAndExpireDate(WebHostingStatus.WaitForPayment, null, dateExpireTo);

            foreach(var hosting in hostings)
            {
                this._webHostingService.PrepareHostingForDeletion(hosting.Id);
                this._notificationManager.SendHostingDisabledEmail(hosting.UserId);
            }
        }
    }
}
