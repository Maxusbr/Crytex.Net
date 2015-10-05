using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Areas.User.Controllers;

namespace Crytex.Web.Controllers.Api
{
    public class StateMachineController : UserCrytexController
    {
        private IStateMachineService _stateMachineService ;
        public StateMachineController(IStateMachineService stateMachineService)
        {
            this._stateMachineService = stateMachineService;
        }

        // GET: api/StateMachine
        public IHttpActionResult Get()
        {
            return Ok(_stateMachineService.GetStateAll());
        }

        // GET: api/StateMachine/5
        public IHttpActionResult Get(int id)
        {
            return Ok(_stateMachineService.GetStateById(id));
        }
    }
}
