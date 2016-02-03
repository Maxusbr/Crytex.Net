using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Background.Config;
using Crytex.Model.Enums;
using Crytex.Notification;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Quartz;

namespace Crytex.Background.Tasks.PhysicalServer
{
    [DisallowConcurrentExecution]
    public class WaitFotPaymentPhysicalServerJob : IJob
    {
        private readonly IBackgroundConfig _config;
        private readonly IPhysicalServerService _physicalServerService;
        private readonly INotificationManager _notificationManager;

        public WaitFotPaymentPhysicalServerJob(IPhysicalServerService physicalServerService, INotificationManager notificationManager, IBackgroundConfig config)
        {
            _physicalServerService = physicalServerService;
            _notificationManager = notificationManager;
            _config = config;
        }

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Wait for payment physical server sub job");
            var srvs = _physicalServerService.GetPhysicalServerByStatus(BoughtPhysicalServerStatus.WaitPayment);
            var currentDate = DateTime.UtcNow;
            var emailPeriod = _config.GetPhysicServerWaitForPaymentPeriod();

            var actionRequiredSrvs = srvs.Where(sub => (currentDate - sub.DateEnd).Days >= emailPeriod);
            foreach (var srv in actionRequiredSrvs)
            {
                _physicalServerService.UpdateBoughtPhysicalServerState(new PhysicalServerStateParams
                {
                    ServerId = srv.Id,
                    State = BoughtPhysicalServerStatus.WaitForDeletion
                });
                _notificationManager.SendMachinePoweredOffEmail(srv.UserId);
            }
        }
    }
}
