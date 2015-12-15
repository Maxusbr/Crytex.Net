using System;
using System.Linq;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using System.Linq.Expressions;
using PagedList;
using System.Data.Entity;

namespace Crytex.Data.Repository
{
   public  class UserVmRepository : RepositoryBase<UserVm>, IUserVmRepository
    {
        public UserVmRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

       public int CountUserVms(Expression<Func<UserVm, bool>> where)
       {
           var count = this.DataContext.UserVms.Where(where).Count();
           return count;
       }
    }
}
