using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.User.Controllers
{
    public class LocationController : UserCrytexController
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        [ResponseType(typeof(IEnumerable<LocationFullViewModel>))]
        public IHttpActionResult Get(int gameId)
        {
            var locations = _locationService.GetLocationsByGameId(gameId);
            var models = Mapper.Map<IEnumerable<LocationFullViewModel>>(locations);

            return Ok(models);
        }
    }
}