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
    public class SendMessagesPhysicalServerJob : IJob
    {
        private readonly IPhysicalServerService _physicalServerService;
        private readonly INotificationManager _notificationManager;

        public SendMessagesPhysicalServerJob(IPhysicalServerService physicalServerService, INotificationManager notificationManager)
        {
            _physicalServerService = physicalServerService;
            _notificationManager = notificationManager;
        }

        public void Execute(IJobExecutionContext context)
        {
            var servers = _physicalServerService.GetPhysicalServerMessageSend();
            foreach (var srv in servers)
            {
                switch (srv.Status)
                {
                    case BoughtPhysicalServerStatus.Created:
                        _notificationManager.SendPhysicalServerAdminCreated(srv.UserId);
                        break;
                    case BoughtPhysicalServerStatus.Active:
                        _notificationManager.SendPhysicalServerAdminReady(srv.UserId, srv.AdminMessage);
                        break;
                    case BoughtPhysicalServerStatus.DontCreate:
                        _notificationManager.SendPhysicalServerAdminDontCreate(srv.UserId, srv.AdminMessage);
                        break;
                }
                _physicalServerService.CompleteSendMessage(srv.Id);
            }

        }
    }
}
