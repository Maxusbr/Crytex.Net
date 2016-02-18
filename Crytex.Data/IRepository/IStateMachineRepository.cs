using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Crytex.Data.Infrastructure;
using Crytex.Model.Models;

namespace Crytex.Data.IRepository
{
    public interface IStateMachineRepository : IRepository<StateMachine>
    {
        List<StateMachine> GetMany<TOrder>(Expression<Func<StateMachine, bool>> where, Expression<Func<StateMachine, TOrder>> order = null);
        StateMachine GetLastState(Guid vmId);
    }
}
