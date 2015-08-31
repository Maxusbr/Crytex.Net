using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class HelpDeskRequestRepository : RepositoryBase<HelpDeskRequest>, IHelpDeskRequestRepository
    {
        public HelpDeskRequestRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
