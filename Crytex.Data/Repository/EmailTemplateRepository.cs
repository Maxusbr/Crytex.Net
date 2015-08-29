using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models.Notifications;

namespace Crytex.Data.Repository
{
    public class EmailTemplateRepository : RepositoryBase<EmailTemplate>, IEmailTemplateRepository
    {
        public EmailTemplateRepository(IDatabaseFactory databaseFactory)
                : base(databaseFactory) { }
    }
}