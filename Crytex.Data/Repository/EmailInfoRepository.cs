using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models.Notifications;

namespace Crytex.Data.Repository
{
    public class EmailInfoRepository : RepositoryBase<EmailInfo>, IEmailInfoRepository
    {
        public EmailInfoRepository(IDatabaseFactory databaseFactory)
                : base(databaseFactory) { }

    }
}