using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models.Biling;

namespace Crytex.Data.Repository
{
    public class UsageSubscriptionPaymentRepository : RepositoryBase<UsageSubscriptionPayment>, IUsageSubscriptionPaymentRepository
    {
        public UsageSubscriptionPaymentRepository(IDatabaseFactory dbFactory) : base(dbFactory) { }
    }
}
