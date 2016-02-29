using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using Crytex.Model.Models.GameServers;

namespace Crytex.Data.Repository
{
    public class GameServerTariffRepository : RepositoryBase<GameServerTariff>, IGameServerTariffRepository
    {
        public GameServerTariffRepository(IDatabaseFactory dbFactory) : base(dbFactory) { }
    }
}
