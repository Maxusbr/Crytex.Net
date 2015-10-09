using Crytex.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crytex.Web.Models.JsonModels;
using Crytex.Model.Models;

namespace Crytex.Web.Controllers.Api
{
    public class VmWareVCenterController : CrytexApiController
    {
        private IVmWareVCenterService _vCenterService;

        public VmWareVCenterController(IVmWareVCenterService vCenterService)
        {
            this._vCenterService = vCenterService;
        }

        // GET api/VmWareVCenter
        [HttpGet]
        public IHttpActionResult Get()
        {
            var vCenters = this._vCenterService.GetAllVCenters();
            var models = AutoMapper.Mapper.Map<List<VmWareVCenterViewModel>>(vCenters);

            return Ok(models);
        }

        // GET api/VmWareVCenter/{id}
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var vCenter = this._vCenterService.GetVCenterById(id);
            var model = AutoMapper.Mapper.Map<VmWareVCenterViewModel>(vCenter);

            return Ok(model);
        }

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

        // PUT api/VmWareCenter/{id}
        [HttpPut]
        public IHttpActionResult Put(int id, VmWareVCenterViewModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return BadRequest(this.ModelState);
            }

            var vCenter = AutoMapper.Mapper.Map<VmWareVCenter>(model);
            this._vCenterService.UpdateVCenter(id, vCenter);

            return Ok();
        }
    }
}
