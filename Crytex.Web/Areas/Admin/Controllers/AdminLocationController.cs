using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminLocationController : AdminCrytexController
    {
        private readonly ILocationService _locationService;

        public AdminLocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var locations = _locationService.GetAllLocations();
            var models = Mapper.Map<IEnumerable<LocationViewModel>>(locations);

            return Ok(models);
        }

        [HttpGet]
        public IHttpActionResult Get(Guid id)
        {
            var location = _locationService.GetById(id);
            var model = Mapper.Map<LocationViewModel>(location);

            return Ok(model);
        }

        [HttpPost]
        public IHttpActionResult Post(LocationViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var location = Mapper.Map<Location>(model);
            location = _locationService.CreateNewLocation(location);

            return Ok(new { Id = location.Id });
        }

        [HttpPut]
        public IHttpActionResult Put(LocationViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var location = Mapper.Map<Location>(model);
            _locationService.UpdateLocation(location);

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            _locationService.Delete(id);

            return Ok();
        }
    }
}