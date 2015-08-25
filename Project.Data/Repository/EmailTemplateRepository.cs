using Project.Data.Infrastructure;
using Project.Data.IRepository;
using Project.Model.Models.Notifications;

namespace Project.Data.Repository
{
    public class EmailTemplateRepository : RepositoryBase<EmailTemplate>, IEmailTemplateRepository
    {
        public EmailTemplateRepository(IDatabaseFactory databaseFactory)
                : base(databaseFactory) { }
    }
}