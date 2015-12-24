using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models.Biling;

namespace Crytex.Data.Repository
{
   public  class SubscriptionVmRepository : RepositoryBase<SubscriptionVm>, ISubscriptionVmRepository
    {
       public SubscriptionVmRepository(DatabaseFactory dbFactory) : base(dbFactory)
       {
           
       }

       public int CountSubscriptionVm(Expression<Func<SubscriptionVm, bool>> where)
       {
           var count = this.DataContext.SubscriptionVms.Where(where).Count();
           return count;
       }
    }
}
