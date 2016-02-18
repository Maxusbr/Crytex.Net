using PagedList;
using Crytex.Model.Models;
using System;
using System.Collections.Generic;

namespace Crytex.Service.IService
{
    public interface IStateMachineService
    {
        IEnumerable<StateMachine> GetStateAll();
        IEnumerable<StateMachine> GetStateByVmId(Guid vmId, int diffInMinutes = 0);
        StateMachine GetLastVmState(Guid vmId);
        StateMachine GetStateById(int id);
        StateMachine CreateState(StateMachine state);
    }
}
