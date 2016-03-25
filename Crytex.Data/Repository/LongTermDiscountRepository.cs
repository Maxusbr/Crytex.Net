using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class LongTermDiscountRepository : RepositoryBase<LongTermDiscount>, ILongTermDiscountRepository
    {
        public LongTermDiscountRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
