using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class BonusReplenishmentRepository : RepositoryBase<BonusReplenishment>, IBonusReplenishmentRepository
    {
        public BonusReplenishmentRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
