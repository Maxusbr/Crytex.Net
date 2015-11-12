using System;
using System.Linq.Expressions;
using Crytex.Data.Infrastructure;
using Crytex.Model.Models;

namespace Crytex.Data.IRepository
{
    public interface IApplicationUserRepository: IRepository<ApplicationUser>
    {
        int CountUsers(Expression<Func<ApplicationUser, bool>> where);
    }
}