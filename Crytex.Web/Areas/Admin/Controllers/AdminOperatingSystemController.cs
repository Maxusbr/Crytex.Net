using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using OperatingSystem = Crytex.Model.Models.OperatingSystem;

namespace Crytex.Web.Areas.Admin
{
    public class AdminOperatingSystemController : AdminCrytexController
    {
        private readonly IOperatingSystemsService _oparaingSystemsService;
        private readonly ApplicationUserManager _userManager;

        public AdminOperatingSystemController(IOperatingSystemsService operatingSystemsService)
        {
            this._oparaingSystemsService = operatingSystemsService;
        }

        // GET: api/OperatingSystem
        public IHttpActionResult Get()
        {
            var systems = this._oparaingSystemsService.GetAll().ToList();
            var model = AutoMapper.Mapper.Map<List<OperatingSystem>, List<OperatingSystemViewModel>>(systems);

            return Ok(model);
        }

        // GET: api/OperatingSystem/5
        public IHttpActionResult Get(int id)
        {
            var os = this._oparaingSystemsService.GeById(id);
            var model = AutoMapper.Mapper.Map<OperatingSystemViewModel>(os);

            return Ok(model);
        }

        // POST: api/OperatingSystem
        public IHttpActionResult Post([FromBody]OperatingSystemEditViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newOS = AutoMapper.Mapper.Map<OperatingSystem>(model);
            newOS = this._oparaingSystemsService.CreateOperatingSystem(newOS);

            return Ok(new { id = newOS.Id });

        }

        // PUT: api/OperatingSystem/5
        public IHttpActionResult Put(int id, [FromBody]OperatingSystemEditViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            var updatedOs = AutoMapper.Mapper.Map<OperatingSystem>(model);
            this._oparaingSystemsService.Update(id, updatedOs);

            return Ok();
        }

        // DELETE: api/OperatingSystem/5
        public IHttpActionResult Delete(int id)
        {
            this._oparaingSystemsService.DeleteById(id);
            return Ok();
        }
    }
}
