using PagedList;
using Crytex.Model.Models;
using System;
using System.Collections.Generic;

namespace Crytex.Service.IService
{
    public interface IStateMachineService
    {
        IEnumerable<StateMachine> GetStateAll();
        StateMachine GetStateById(int id);
        StateMachine CreateState(StateMachine state);
    }
}
