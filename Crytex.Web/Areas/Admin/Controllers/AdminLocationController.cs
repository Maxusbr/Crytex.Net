using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminLocationController : AdminCrytexController
    {
        private readonly ILocationService _locationService;
        private readonly IGameHostService _gameHostService;

        public AdminLocationController(ILocationService locationService, IGameHostService gameHostService)
        {
            _locationService = locationService;
            _gameHostService = gameHostService;
        }

        [HttpGet]
        [ResponseType(typeof(IEnumerable<LocationFullViewModel>))]
        public IHttpActionResult Get(int? gameId = null)
        {
            var locations = _locationService.GetLocationsByGameId(gameId);
            var models = Mapper.Map<IEnumerable<LocationViewModel>>(locations);

            return Ok(models);
        }

        [HttpGet]
        [ResponseType(typeof(LocationViewModel))]
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