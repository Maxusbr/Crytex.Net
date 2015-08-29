using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using OperatingSystem = Crytex.Model.Models.OperatingSystem;

namespace Crytex.Data.Repository
{
    public class OperatingSystemRepository : RepositoryBase<OperatingSystem>, IOperatingSystemRepository
    {
        public OperatingSystemRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
