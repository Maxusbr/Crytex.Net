using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models.Biling;

namespace Crytex.Data.Repository
{
    public class WebHostingPaymentRepository : RepositoryBase<WebHostingPayment>, IWebHostingPaymentRepository
    {
        public WebHostingPaymentRepository(IDatabaseFactory dbFactory) : base(dbFactory) { }
    }
}
