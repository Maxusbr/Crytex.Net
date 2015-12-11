using System;
using Crytex.Model.Models;
using Crytex.Service.IService;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.Admin
{
    public class AdminTriggerController : AdminCrytexController
    {
        private readonly ITriggerService _TriggerService;

        public AdminTriggerController(ITriggerService triggerService)
        {
            this._TriggerService = triggerService;
        }

        /// <summary>
        /// Получение всех Trigger
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        // GET: api/Trigger
        [ResponseType(typeof(IEnumerable<TriggerViewModel>))]
        public IHttpActionResult Get(int pageNumber, int pageSize, string userId = null)
        {
            var Triggers = this._TriggerService.GetPage(pageNumber, pageSize, userId);
            var viewModel = AutoMapper.Mapper.Map<IEnumerable<TriggerViewModel>>(Triggers);

            return Ok(viewModel);
        }

        /// <summary>
        /// Получение Trigger по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Trigger/5
        [ResponseType(typeof(TriggerViewModel))]
        public IHttpActionResult Get(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                this.ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }

            var trigger = this._TriggerService.GetTriggerById(guid);
            var viewModel = AutoMapper.Mapper.Map<TriggerViewModel>(trigger);

            return Ok(viewModel);
        }

        /// <summary>
        /// Создание нового Trigger
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: api/Trigger
        public IHttpActionResult Post([FromBody]TriggerViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var trigger = AutoMapper.Mapper.Map<Trigger>(model);
            var newTrigger = this._TriggerService.CreateTrigger(trigger);

            return Ok(newTrigger);
        }

        /// <summary>
        /// Обновление Trigger
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        // PUT: api/Trigger/5
        public IHttpActionResult Put(string id,[FromBody]TriggerViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                this.ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }

            var trigger = AutoMapper.Mapper.Map<Trigger>(model);

            this._TriggerService.UpdateTrigger(trigger);

            return Ok();
        }

        /// <summary>
        /// Удаление Trigger по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Trigger/5
        public IHttpActionResult Delete(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                this.ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }

            this._TriggerService.DeleteTriggerById(guid);
            return Ok();
        }
    }
}
