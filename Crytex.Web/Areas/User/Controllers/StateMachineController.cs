﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using Microsoft.Practices.Unity;

namespace Crytex.Web.Areas.User
{
    public class StateMachineController : UserCrytexController
    {
        private IStateMachineService _stateMachineService ;
        public StateMachineController([Dependency("Secured")]IStateMachineService stateMachineService)
        {
            this._stateMachineService = stateMachineService;
        }

        /// <summary>
        /// Получение списка состояний машин
        /// </summary>
        /// <returns></returns>
        // GET: api/StateMachine
        [ResponseType(typeof(IEnumerable<StateMachine>))]
        public IHttpActionResult Get(Guid vmId, int diffTime = 0)
        {
            return Ok(_stateMachineService.GetStateByVmId(vmId, diffTime));
        }

        /// <summary>
        /// Получение состояния машины по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/StateMachine/5
        [ResponseType(typeof(StateMachine))]
        public IHttpActionResult Get(int id)
        {
            return Ok(_stateMachineService.GetStateById(id));
        }

        [HttpPost]
        public IHttpActionResult GetLastState(Guid vmId)
        {
            var state = this._stateMachineService.GetLastVmState(vmId);
            return this.Ok(state);
        }
    }
}
