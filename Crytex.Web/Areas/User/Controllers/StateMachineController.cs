using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;


namespace Crytex.Web.Areas.User
{
    public class StateMachineController : UserCrytexController
    {
        private IStateMachineService _stateMachineService ;
        public StateMachineController(IStateMachineService stateMachineService)
        {
            this._stateMachineService = stateMachineService;
        }

        // GET: api/StateMachine
        [ResponseType(typeof(IEnumerable<StateMachine>))]
        public IHttpActionResult Get()
        {
            return Ok(_stateMachineService.GetStateAll());
        }

        // GET: api/StateMachine/5
        [ResponseType(typeof(StateMachine))]
        public IHttpActionResult Get(int id)
        {
            return Ok(_stateMachineService.GetStateById(id));
        }
    }
}
