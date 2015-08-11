using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Project.Service.IService;
using Project.Web.Models.JsonModels;
using OperatingSystem = Project.Model.Models.OperatingSystem;

namespace Project.Web.Controllers.Api
{
    public class OperatingSystemController : ApiController
    {
        private readonly IOperatingSystemsService _oparaingSystemsService;
        private readonly ApplicationUserManager _userManager;

        public OperatingSystemController(IOperatingSystemsService operatingSystemsService, ApplicationUserManager userManager)
        {
            this._userManager = userManager;
            this._oparaingSystemsService = operatingSystemsService;
        }

        // GET: api/OperatingSystem
        public IEnumerable<OperatingSystemViewModel> GetAll()
        {
            var systems = this._oparaingSystemsService.GetAll().ToList();
            var model = AutoMapper.Mapper.Map<List<OperatingSystem>, List<OperatingSystemViewModel>>(systems);

            return model;
        }

        // GET: api/OperatingSystem/5
        [HttpGet]
        public OperatingSystemViewModel GetById(int id)
        {
            var os = this._oparaingSystemsService.GeById(id);
            var model = AutoMapper.Mapper.Map<OperatingSystemViewModel>(os);

            return model;
        }

        // POST: api/OperatingSystem
        [HttpPost]
        public HttpResponseMessage Create(OperatingSystemEditViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var newOS = AutoMapper.Mapper.Map<OperatingSystem>(model);
                newOS = this._oparaingSystemsService.CreateOperatingSystem(newOS);

                return Request.CreateResponse(HttpStatusCode.Created, new { id = newOS.Id });
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
        }

        // PUT: api/OperatingSystem/5
        [HttpPut]
        public HttpResponseMessage Update(int id, OperatingSystemEditViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var updatedOs = AutoMapper.Mapper.Map<OperatingSystem>(model);
                this._oparaingSystemsService.Update(id, updatedOs);

                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
        }

        // DELETE: api/OperatingSystem/5
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            this._oparaingSystemsService.DeleteById(id);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
