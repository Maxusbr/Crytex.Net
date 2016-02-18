using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class StateMachineRepository : RepositoryBase<StateMachine>, IStateMachineRepository
    {
        public StateMachineRepository(DatabaseFactory dbFacrory) : base(dbFacrory)
        {
            
        }

        public StateMachine GetLastState(Guid vmId)
        {
            var statesQuery = this.DataContext.StateMachines.Where(s => s.VmId == vmId);
            StateMachine state = null;
            if (statesQuery.Any())
            {
                var maxDate = statesQuery.Max(s => s.Date);
                state = this.DataContext.StateMachines.Where(s => s.VmId == vmId && s.Date == maxDate).Single();
            }

            return state;
        }

        public List<StateMachine> GetMany<TOrder>(Expression<Func<StateMachine, bool>> where, Expression<Func<StateMachine, TOrder>> order = null)
        {
            var query = this.DataContext.StateMachines.Where(where);
            if (order != null)
            {
                query = query.OrderBy(order);
            }
            //query = this.AppendIncludes(query, includes);

            return query.ToList();
        }
    }
}
