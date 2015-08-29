using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class ServerTemplateRepository : RepositoryBase<ServerTemplate>, IServerTemplateRepository
    {
        public ServerTemplateRepository(IDatabaseFactory dataBaseFactory) : base(dataBaseFactory) { }
    }
}
