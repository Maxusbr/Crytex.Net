using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class TriggerRepository : RepositoryBase<Trigger>, ITriggerRepository
    {
        public TriggerRepository(IDatabaseFactory dbFacrory) : base(dbFacrory){ }
    }
}
