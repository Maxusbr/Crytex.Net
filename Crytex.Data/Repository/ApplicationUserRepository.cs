using System;
using System.Linq;
using System.Linq.Expressions;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class ApplicationUserRepository : RepositoryBase<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(IDatabaseFactory databaseFactory)
                : base(databaseFactory) { }
        public int CountUsers(Expression<Func<ApplicationUser, bool>> where)
        {
            return this.DataContext.Users.Where(where).Count();
        }
    }

}