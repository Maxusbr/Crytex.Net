using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
   public  class UpdateTaskVmRepository : RepositoryBase<UpdateVmTask>, IUpdateVmTaskRepository
    {
        public UpdateTaskVmRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
