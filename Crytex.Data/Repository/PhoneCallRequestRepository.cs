using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class PhoneCallRequestRepository : RepositoryBase<PhoneCallRequest>, IPhoneCallRequestRepository
    {
        public PhoneCallRequestRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
