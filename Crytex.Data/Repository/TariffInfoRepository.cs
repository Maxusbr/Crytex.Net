using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class TariffInfoRepository : RepositoryBase<Tariff>, ITariffInfoRepository
    {
        public TariffInfoRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }
}
