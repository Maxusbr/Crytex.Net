using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models.WebHostingModels;

namespace Crytex.Data.Repository
{
    public class HostedWebApplicationRepository : RepositoryBase<HostedWebApplication>, IHostedWebApplicationRepository
    {
        public HostedWebApplicationRepository(IDatabaseFactory dbFactory) : base(dbFactory) { }
    }
}
