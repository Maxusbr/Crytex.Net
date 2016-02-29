using System.Collections;
using System.Collections.Generic;
using Crytex.Model.Models.GameServers;

namespace Crytex.Service.IService
{
    public interface IGameService
    {
        Game Create(Game game);
        IEnumerable<Game> GetGamesByIds(IEnumerable<int> gameIds);
    }
}