using Crytex.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crytex.Web.Models.JsonModels;
using Crytex.Model.Models;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminVmWareVCenterController : AdminCrytexController
    {
        private IVmWareVCenterService _vCenterService;

        public AdminVmWareVCenterController(IVmWareVCenterService vCenterService)
        {
            this._vCenterService = vCenterService;
        }

        /// <summary>
        /// Получения списка машин
        /// </summary>
        /// <returns></returns>
        // GET api/VmWareVCenter
        [HttpGet]
        public IHttpActionResult Get()
        {
            var vCenters = this._vCenterService.GetAllVCenters();
            var models = AutoMapper.Mapper.Map<List<VmWareVCenterViewModel>>(vCenters);

            return Ok(models);
        }

        /// <summary>
        /// Получения машины по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/VmWareVCenter/{id}
        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                this.ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }

            var vCenter = this._vCenterService.GetVCenterById(guid);
            var model = AutoMapper.Mapper.Map<VmWareVCenterViewModel>(vCenter);

            return Ok(model);
        }

        /// <summary>
        /// Создание машины
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST api/VmWareVCenter
        [HttpPost]
        public IHttpActionResult Post([FromBody]VmWareVCenterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newCenter = AutoMapper.Mapper.Map<VmWareVCenter>(model);
            newCenter = this._vCenterService.CreateVCenter(newCenter);

            return Ok(new { id = newCenter.Id });
        }

        /// <summary>
        /// Обновление машины
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        // PUT api/VmWareCenter/{id}
        [HttpPut]
        public IHttpActionResult Put(string id, [FromBody]VmWareVCenterViewModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return BadRequest(this.ModelState);
            }

            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                this.ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }

            var vCenter = AutoMapper.Mapper.Map<VmWareVCenter>(model);
            this._vCenterService.UpdateVCenter(guid, vCenter);

            return Ok();
        }
    }
}
