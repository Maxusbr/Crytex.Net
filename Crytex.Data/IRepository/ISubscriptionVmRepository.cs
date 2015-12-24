using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models.Biling;
using Crytex.Data.Infrastructure;

namespace Crytex.Data.IRepository
{
    public interface ISubscriptionVmRepository : IRepository<SubscriptionVm>
    {
        int CountSubscriptionVm(Expression<Func<SubscriptionVm, bool>> where);
    }
}
