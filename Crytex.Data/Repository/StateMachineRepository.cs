using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using Crytex.Notification;

namespace Crytex.Data.Repository
{
    public class StateMachineRepository : RepositoryBase<StateMachine>, IStateMachineRepository
    {
        public StateMachineRepository(DatabaseFactory dbFacrory) : base(dbFacrory){ }
    }
}
