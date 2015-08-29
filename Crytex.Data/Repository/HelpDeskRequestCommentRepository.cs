using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class HelpDeskRequestCommentRepository : RepositoryBase<HelpDeskRequestComment>, IHelpDeskRequestCommentRepository
    {
        public HelpDeskRequestCommentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
