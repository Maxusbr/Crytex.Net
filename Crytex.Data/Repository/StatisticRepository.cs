using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class StatisticRepository : RepositoryBase<Statistic>, IStatisticRepository
    {
        public StatisticRepository(IDatabaseFactory dbFacrory) : base(dbFacrory){ }
    }
}
