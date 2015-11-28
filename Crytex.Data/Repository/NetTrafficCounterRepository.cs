using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class NetTrafficCounterRepository : RepositoryBase<NetTrafficCounter>, INetTrafficCounterRepository
    {
        public NetTrafficCounterRepository(IDatabaseFactory dbFactory) : base(dbFactory) { }
    }
}
