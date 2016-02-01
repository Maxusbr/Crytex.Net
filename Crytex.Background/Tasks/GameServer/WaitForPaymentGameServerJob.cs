using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Background.Config;
using Crytex.Model.Models.Biling;
using Crytex.Notification;
using Crytex.Service.IService;
using Quartz;

namespace Crytex.Background.Tasks.GameServer
{
    [DisallowConcurrentExecution]
    public class WaitForPaymentGameServerJob: IJob
    {
        private readonly IGameServerService _gameServerService;
        private readonly IBackgroundConfig _config;
        private readonly INotificationManager _notificationManager;

        public WaitForPaymentGameServerJob(IGameServerService gameServerService, IBackgroundConfig config, INotificationManager notificationManager)
        {
            _gameServerService = gameServerService;
            _config = config;
            _notificationManager = notificationManager;
        }

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Wait for payment sub job");
            var servers = _gameServerService.GetGameServerByStatus(GameServerStatus.WaitForPayment);
            var currentDate = DateTime.UtcNow;
            var warnPeriod = _config.GetGameServerEndWarnPeriod();

            var actionRequiredServer = servers.Where(srv => (currentDate - (srv.DateEnd ?? new DateTime())).Days >= warnPeriod);
            foreach (var srv in actionRequiredServer)
                _gameServerService.DeleteGameServer(srv.GameServerId);
        }
    }
}
