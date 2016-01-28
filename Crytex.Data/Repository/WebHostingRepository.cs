using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models.WebHosting;

namespace Crytex.Data.Repository
{
    public class WebHostingRepository : RepositoryBase<WebHosting>, IWebHostingRepository
    {
        public WebHostingRepository(IDatabaseFactory dbFactory) : base(dbFactory) { }
    }
}
