using System;
using System.Collections;
using System.Collections.Generic;
using Crytex.Model.Models;
using Crytex.Model.Models.Biling;
using Crytex.Service.Model;
using PagedList;

namespace Crytex.Service.IService
{
    public interface IGameServerService
    {
        /// <summary>
        /// Создаёт новый игровой сервер, задачу создания виртуальной машины, а также саму сущность виртуальной машины в БД
        /// </summary>
        GameServer CreateServer(GameServer server);
        GameServer GetById(Guid guid);
        IPagedList<GameServer> GetPage(int pageNumber, int pageSize, string userId = null);
        IEnumerable<GameServer> GetAllByUserId(string userId);

        GameServer BuyGameServer(GameServer server, BuyGameServerOption option);
        decimal GetGameServerMonthPrice(GameServer server);
        IPagedList<PaymentGameServer> GetPage(int pageNumber, int pageSize, SearchPaymentGameServerParams filter = null);

        IEnumerable<PaymentGameServer> GetGameServerByStatus(GameServerStatus status);
        void UpdateStatusServer(Guid gameServerId, GameServerStatus waitForPayment);
        void DeleteGameServer(Guid gameServerId);
        IEnumerable<PaymentGameServer>  GetAllGameServers();
        void UpdateGameServer(Guid serverId, GameServerConfigOptions options);
        void AutoProlongateGameServer(Guid gameServerId);

        void StartGameServer(Guid serverId);
        void StopGameServer(Guid serverId);
        void PowerOffGameServer(Guid serverId);
        void ResetGameServer(Guid serverId);
        void UpdateGameServerMachineConfig(Guid gameServerId, UpdateMachineConfigOptions serviceOptions);
    }
}
