using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class BillingTransactionRepository : RepositoryBase<BillingTransaction>, IBillingTransactionRepository
    {
        public BillingTransactionRepository(DatabaseFactory dbFactory) : base(dbFactory) { }
    }
}
