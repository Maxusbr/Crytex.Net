using System;
using System.Linq;
using System.Linq.Expressions;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class TaskV2Repository : RepositoryBase<TaskV2>, ITaskV2Repository
    {
        public TaskV2Repository(IDatabaseFactory dbFacrory) : base(dbFacrory){ }

        public int CountTaskV2(Expression<Func<TaskV2, bool>> where)
        {
            return this.DataContext.TaskV2.Where(where).Count();
        }
    }
}
