using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class StandartVmTaskRepository : RepositoryBase<StandartVmTask>,IStandartVmTaskRepository
    {
        public StandartVmTaskRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
