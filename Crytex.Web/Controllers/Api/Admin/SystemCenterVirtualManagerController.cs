using Crytex.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crytex.Web.Models.JsonModels;
using Crytex.Model.Models;

namespace Crytex.Web.Controllers.Api.Admin
{
    public class SystemCenterVirtualManagerController : CrytexApiController
    {
        private ISystemCenterVirtualManagerService _managerService;

        public SystemCenterVirtualManagerController(ISystemCenterVirtualManagerService managerService)
        {
            this._managerService = managerService;
        }

        /// <summary>
        /// Получение менеджера по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult Get(string id)
        {
            var manager = this._managerService.GetById(id);
            var model = AutoMapper.Mapper.Map<SystemCenterVirtualManagerViewModel>(manager);

            return Ok(model);
        }

        /// <summary>
        /// Получение всех менеджеров
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult Get()
        {
            var managers = this._managerService.GetAll(false);
            var model = AutoMapper.Mapper.Map<System.Collections.Generic.List<SystemCenterVirtualManagerViewModel>>(managers);

            return Ok(model);
        }

        /// <summary>
        /// Создание нового менеджера в БД
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IHttpActionResult Post([FromBody]SystemCenterVirtualManagerViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(this.ModelState);
            }
            var newManager = AutoMapper.Mapper.Map<SystemCenterVirtualManager>(model);
            newManager = this._managerService.Create(newManager);

            return Created(Url.Link("DefaultApi", new { controller = "SystemCenterVirtualManager", id = newManager.Id.ToString() }), new { id = newManager.Id.ToString() });

            
        }

        /// <summary>
        /// Удаление менеджера по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult Delete(string id)
        {
            this._managerService.Delete(id);

            return Ok();
        }
    }
}
