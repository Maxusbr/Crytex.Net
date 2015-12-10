using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class UserLoginLogEntryRepository : RepositoryBase<UserLoginLogEntry>, IUserLoginLogEntryRepository
    {
        public UserLoginLogEntryRepository(IDatabaseFactory dbFactory) : base(dbFactory) { }
    }
}
