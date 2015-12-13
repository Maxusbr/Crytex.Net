using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class GameServerConfigurationRepository : RepositoryBase<GameServerConfiguration>, IGameServerConfigurationRepository
    {
        public GameServerConfigurationRepository(IDatabaseFactory dbFactory) : base(dbFactory) { }
    }
}
