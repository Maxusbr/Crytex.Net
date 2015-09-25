using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crytex.Model.Models;
using Crytex.Service.IService;

namespace Crytex.Web.Controllers.Api
{
    public class StateMachineController : ApiController
    {
        private IStateMachineService _stateMachineService ;
        public StateMachineController(IStateMachineService stateMachineService)
        {
            this._stateMachineService = stateMachineService;
        }

        // GET: api/StateMachine
        public IEnumerable<StateMachine> Get()
        {
            return _stateMachineService.GetStateAll();
        }

        // GET: api/StateMachine/5
        public StateMachine Get(int id)
        {
            return _stateMachineService.GetStateById(id);
        }
    }
}
