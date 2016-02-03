using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Background.Config;
using Crytex.Model.Enums;
using Crytex.Notification;
using Crytex.Service.IService;
using Quartz;

namespace Crytex.Background.Tasks.PhysicalServer
{
    [DisallowConcurrentExecution]
    public class UsagePhysicalServerJob : IJob
    {
        private readonly IPhysicalServerService _physicalServerService;
        private readonly INotificationManager _notificationManager;
        private readonly IBackgroundConfig _config;

        public UsagePhysicalServerJob(IPhysicalServerService physicalServerService, INotificationManager notificationManager, 
            IBackgroundConfig config)
        {
            _physicalServerService = physicalServerService;
            _notificationManager = notificationManager;
            _config = config;
        }

        public void Execute(IJobExecutionContext context)
        {
            var srvrs = _physicalServerService.GetAllUsagePhysicalServer();
            var subscriptionEndWarnPeriod = _config.GetPhysicServerWaitForPaymentPeriod();
            var deletionPeriod = _config.GetPhysicServerWaitForDeletionPeriod();
            var currentDate = DateTime.UtcNow;

            foreach (var srv in srvrs)
            {
                var daysToEnd = (currentDate - srv.DateEnd).Days;
                if (currentDate > srv.DateEnd && daysToEnd == subscriptionEndWarnPeriod)
                {
                    _notificationManager.SendPhysicalServerEndWarningEmail(srv.UserId, subscriptionEndWarnPeriod);
                }
                if (srv.Status == BoughtPhysicalServerStatus.WaitForDeletion)
                {
                    var daysToDeletion = (srv.DateEnd.AddDays(deletionPeriod) - currentDate).Days;
                    _notificationManager.SendPhysicalServerDeletionWarningEmail(srv.UserId, daysToDeletion);
                }
            }
        }
    }
}
