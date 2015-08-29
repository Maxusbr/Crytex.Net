using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class CreateVmTaskRepository : RepositoryBase<CreateVmTask>, ICreateVmTaskRepository
    {
        public CreateVmTaskRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
