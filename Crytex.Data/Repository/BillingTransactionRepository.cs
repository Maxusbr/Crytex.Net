using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using Crytex.Model.Models.Biling;

namespace Crytex.Data.Repository
{
    public class BillingTransactionRepository : RepositoryBase<BillingTransaction>, IBillingTransactionRepository
    {
        public BillingTransactionRepository(DatabaseFactory dbFactory) : base(dbFactory) { }
    }
}
