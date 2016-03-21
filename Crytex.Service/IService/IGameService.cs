using System;
using System.Collections;
using System.Collections.Generic;
using Crytex.Model.Enums;
using Crytex.Model.Models.GameServers;
using PagedList;

namespace Crytex.Service.IService
{
    public interface IGameService
    {
        Game Create(Game game);
        Game GetById(int id);
        IEnumerable<Game> GetGamesByIds(IEnumerable<int> gameIds);
        IPagedList<Game> GetPage(int pageNumber, int pageSize, GameFamily family);
        void Update(Game game);
        void Delete(Int32 Id);
        IEnumerable<GameServerTariff> GetLastTariffsForGames(IEnumerable<int> gamesIds);
    }
}