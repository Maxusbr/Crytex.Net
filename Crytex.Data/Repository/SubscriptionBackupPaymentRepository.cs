using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models.Biling;

namespace Crytex.Data.Repository
{
    public class SubscriptionBackupPaymentRepository : RepositoryBase<SubscriptionVmBackupPayment>, ISubscriptionBackupPaymentRepository
    {
        public SubscriptionBackupPaymentRepository(IDatabaseFactory dbFactory) : base(dbFactory) { }
    }
}
