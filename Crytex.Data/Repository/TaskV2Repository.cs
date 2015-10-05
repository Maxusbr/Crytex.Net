using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class TaskV2Repository : RepositoryBase<TaskV2>, ITaskV2Repository
    {
        public TaskV2Repository(DatabaseFactory dbFacrory) : base(dbFacrory){ }
    }
}
