using System;
using System.Linq.Expressions;
using Crytex.Data.Infrastructure;
using Crytex.Model.Models;
using Crytex.Model.Models.Biling;

namespace Crytex.Data.IRepository
{
    public interface IBillingTransactionRepository : IRepository<BillingTransaction>
    {
        int CountBillingTransaction(Expression<Func<BillingTransaction, bool>> where);
    }
}
