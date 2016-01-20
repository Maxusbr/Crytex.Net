﻿using System;
using Crytex.Model.Models;
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
    }
}
