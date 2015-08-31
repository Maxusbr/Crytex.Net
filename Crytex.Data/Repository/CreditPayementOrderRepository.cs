using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class CreditPayementOrderRepository : RepositoryBase<CreditPaymentOrder>, ICreditPaymentOrderRepository
    {
        public CreditPayementOrderRepository(DatabaseFactory dbFactory) : base(dbFactory) { }
    }
}
