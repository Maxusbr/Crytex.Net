using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class RegionRepository : RepositoryBase<Region>, IRegionRepository
    {
        public RegionRepository(IDatabaseFactory dbFacrory) : base(dbFacrory){ }
    }
}
