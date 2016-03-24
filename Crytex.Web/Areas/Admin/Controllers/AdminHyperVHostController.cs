using System;
using System.Collections.Generic;
using System.Web.Http;
using Crytex.Service.IService;
using AutoMapper;
using Crytex.Model.Models;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminHyperVHostController : AdminCrytexController
    {
        private readonly IHyperVHostService _hostService;

        public AdminHyperVHostController(IHyperVHostService hostService)
        {
            _hostService = hostService;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var hosts = _hostService.GetAllHyperVHosts();
            var models = Mapper.Map<IEnumerable<HyperVHostViewModel>>(hosts);

            return Ok(models);
        }

        [HttpGet]
        public IHttpActionResult Get(Guid id)
        {
            var host = _hostService.GetHyperVById(id);
            var model = Mapper.Map<HyperVHostViewModel>(host);

            return Ok(model);
        }

        [HttpPost]
        public IHttpActionResult Post(HyperVHostViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var host = Mapper.Map<HyperVHost>(model);
            host.CreatedManual = true;
            host = _hostService.CreateHyperVHost(host);

            return Ok(new {Id = host.Id});
        }

        [HttpPut]
        public IHttpActionResult Put(HyperVHostViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var host = Mapper.Map<HyperVHost>(model);
            _hostService.UpdateHyperVHost(host);

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            _hostService.DeleteHost(id);

            return Ok();
        }
    }
}