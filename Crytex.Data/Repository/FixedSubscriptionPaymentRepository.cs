using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models.Biling;

namespace Crytex.Data.Repository
{
    public class FixedSubscriptionPaymentRepository : RepositoryBase<FixedSubscriptionPayment>, IFixedSubscriptionPaymentRepository
    {
        public FixedSubscriptionPaymentRepository(IDatabaseFactory dbFactory) : base(dbFactory) { }
    }
}
