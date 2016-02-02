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
    public class WaiForDeletionPhysicalServerJob : IJob
    {
        private readonly IBackgroundConfig _config;
        private readonly IPhysicalServerService _physicalServerService;

        public WaiForDeletionPhysicalServerJob(IPhysicalServerService physicalServerService, IBackgroundConfig config)
        {
            _physicalServerService = physicalServerService;
            _config = config;
        }

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Wait for deletion physical server sub job");
            var srvs = _physicalServerService.GetPhysicalServerByStatus(BoughtPhysicalServerStatus.WaitForDeletion);
            var currentDate = DateTime.UtcNow;
            var emailPeriod = _config.GetPhysicServerWaitForDeletionPeriod();

            var actionRequiredSrvs = srvs.Where(sub => (currentDate - sub.DateEnd).Days >= emailPeriod);
            foreach (var srv in actionRequiredSrvs)
            {
                _physicalServerService.DeleteBoughtPhysicalServer(srv.Id);
            }
        }
    }
}
