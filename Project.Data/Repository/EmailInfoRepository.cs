using Project.Data.Infrastructure;
using Project.Data.IRepository;
using Project.Model.Models.Notifications;

namespace Project.Data.Repository
{
    public class EmailInfoRepository : RepositoryBase<EmailInfo>, IEmailInfoRepository
    {
        public EmailInfoRepository(IDatabaseFactory databaseFactory)
                : base(databaseFactory) { }

    }
}