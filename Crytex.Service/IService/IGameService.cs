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
        IEnumerable<Game> GetGamesByIds(IEnumerable<int> gameIds);
        IPagedList<Game> GetPage(int pageNumber, int pageSize, GameFamily family);
        void Update(Game game);
    }
}