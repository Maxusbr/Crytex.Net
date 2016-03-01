using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models.Biling;
using Crytex.Model.Models.GameServers;
using Crytex.Notification;
using Crytex.Service.IService;
using Quartz;

namespace Crytex.Background.Tasks.GameServer
{
    [DisallowConcurrentExecution]
    public class ActiveGameServerJob : IJob
    {
        private readonly IGameServerService _gameServerService;
        private readonly INotificationManager _notificationManager;

        public ActiveGameServerJob(IGameServerService gameServerService, INotificationManager notificationManager)
        {
            _gameServerService = gameServerService;
            _notificationManager = notificationManager;
        }

        public void Execute(IJobExecutionContext context)
        {
            var servers = _gameServerService.GetGameServerByStatus(GameServerStatus.Active);
            var currentDate = DateTime.UtcNow;

            var outdateServers = servers.Where(o => o.DateExpire < currentDate);
            foreach (var srv in outdateServers)
                if (srv.AutoProlongation)
                    _gameServerService.AutoProlongateGameServer(srv.Id);
                else
                {
                    _gameServerService.UpdateStatusServer(srv.Id, GameServerStatus.WaitForPayment);
                    _notificationManager.SendGameServerPoweredOffEmail(srv.UserId);
                }
        }
    }
}
