using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Background.Config;
using Crytex.Model.Models.Biling;
using Crytex.Model.Models.GameServers;
using Crytex.Notification;
using Crytex.Service.IService;
using Quartz;

namespace Crytex.Background.Tasks.GameServer
{
    [DisallowConcurrentExecution]
    public class WarningsGameServerJob : IJob
    {
        private readonly IBackgroundConfig _config;
        private readonly INotificationManager _notificationManager;
        private readonly IGameServerService _gameServerService;

        public WarningsGameServerJob(IBackgroundConfig config, INotificationManager notificationManager, IGameServerService gameServerService)
        {
            _config = config;
            _notificationManager = notificationManager;
            _gameServerService = gameServerService;
        }

        public void Execute(IJobExecutionContext context)
        {
            var servers = _gameServerService.GetAllGameServers();
            var serverEndWarnPeriod = _config.GetGameServerEndWarnPeriod();
            var deletionPeriod = _config.GetGameServerWaitForPaymentPeriod();
            var currentDate = DateTime.UtcNow;

            foreach (var srv in servers)
            {
                var srvDate = srv.DateExpire;
                var daysToEnd = (srvDate - currentDate).Days;
                if (currentDate < srvDate && daysToEnd == serverEndWarnPeriod)
                {
                    _notificationManager.SendGameServerEndWarningEmail(srv.UserId, serverEndWarnPeriod);
                }
                if (srv.Status == GameServerStatus.WaitForPayment)
                {
                    var daysToDeletion = (srvDate.AddDays(deletionPeriod) - currentDate).Days;
                    _notificationManager.SendGameServerDeletionWarningEmail(srv.UserId, daysToDeletion);
                }
            }
        }
    }
}
