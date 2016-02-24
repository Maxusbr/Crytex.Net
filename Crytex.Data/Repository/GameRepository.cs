using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models.GameServers;

namespace Crytex.Data.Repository
{
    public class GameRepository : RepositoryBase<Game>, IGameRepository
    {
        public GameRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}