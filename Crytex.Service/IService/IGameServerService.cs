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

        GameServer BuyGameServer(GameServer server, BuyGameServerOption option);

        IPagedList<PaymentGameServer> GetPage(int pageNumber, int pageSize, SearchPaymentGameServerParams filter = null);

        IEnumerable<PaymentGameServer> GetGameServerByStatus(GameServerStatus status);
        void UpdateStatusServer(Guid gameServerId, GameServerStatus waitForPayment);
        void DeleteGameServer(Guid gameServerId);
        IEnumerable<PaymentGameServer>  GetAllGameServers();
        void AutoProlongateGameServer(Guid gameServerId);
    }
}
