using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using Crytex.Model.Models.Biling;

namespace Crytex.Data.Repository
{
    public class CreditPayementOrderRepository : RepositoryBase<Payment>, ICreditPaymentOrderRepository
    {
        public CreditPayementOrderRepository(DatabaseFactory dbFactory) : base(dbFactory) { }
    }
}
