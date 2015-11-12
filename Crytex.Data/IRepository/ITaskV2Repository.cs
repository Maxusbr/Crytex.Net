using System;
using System.Linq.Expressions;
using Crytex.Model.Models;
using Crytex.Data.Infrastructure;

namespace Crytex.Data.IRepository
{
    public interface ITaskV2Repository : IRepository<TaskV2>
    {
        int CountTaskV2(Expression<Func<TaskV2, bool>> where);
    }
}
