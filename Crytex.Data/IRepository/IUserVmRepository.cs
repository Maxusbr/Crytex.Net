using System;
using System.Linq.Expressions;
using Crytex.Model.Models;
using Crytex.Data.Infrastructure;

namespace Crytex.Data.IRepository
{
    public interface IUserVmRepository : IRepository<UserVm>
    {
        int CountUserVms(Expression<Func<UserVm, bool>> where);
    }
}
