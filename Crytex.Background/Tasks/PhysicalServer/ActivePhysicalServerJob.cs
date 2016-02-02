using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Background.Config;
using Crytex.Model.Enums;
using Crytex.Model.Models.Biling;
using Crytex.Notification;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Quartz;

namespace Crytex.Background.Tasks.PhysicalServer
{
    [DisallowConcurrentExecution]
    public class ActivePhysicalServerJob : IJob
    {
        private readonly IBackgroundConfig _config;
        private readonly IPhysicalServerService _physicalServerService;
        private readonly INotificationManager _notificationManager;

        public ActivePhysicalServerJob(IPhysicalServerService physicalServerService, INotificationManager notificationManager,
            IBackgroundConfig config)
        {
            _physicalServerService = physicalServerService;
            _notificationManager = notificationManager;
            _config = config;
        }

        public void Execute(IJobExecutionContext context)
        {
            var servers = _physicalServerService.GetPhysicalServerByStatus(BoughtPhysicalServerStatus.Active);
            var currentDate = DateTime.UtcNow;

            var outdateServers = servers.Where(o => o.DateEnd < currentDate);
            foreach (var srv in outdateServers)
                if (srv.AutoProlongation)
                    _physicalServerService.AutoProlongatePhysicalServer(srv.Id);
                else
                {
                    _physicalServerService.UpdateBoughtPhysicalServerState(new PhysicalServerStateParams
                    {
                        ServerId = srv.Id,
                        State = BoughtPhysicalServerStatus.WaitPayment
                    });
                    _notificationManager.SendPhysicalServerWaitPaymentEmail(srv.UserId);
                }
        }
    }
}
