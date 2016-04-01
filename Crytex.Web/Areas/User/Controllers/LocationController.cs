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
        private readonly IGameHostService _gameHostService;

        public LocationController(IGameHostService gameHostService)
        {
            _gameHostService = gameHostService;
        }

        [HttpGet]
        [ResponseType(typeof(IEnumerable<LocationFullViewModel>))]
        public IHttpActionResult Get(int gameId)
        {
            var hosts = _gameHostService.GetGameHostsByGameId(gameId).ToList();
            var models =
                    Mapper.Map<IEnumerable<LocationFullViewModel>>(hosts.GroupBy(o => o.Location).Select(x => x.Key));
            var locationFullViewModels = models as IList<LocationFullViewModel> ?? models.ToList();
            foreach (var el in locationFullViewModels)
            {
                el.GameHosts = Mapper.Map<IEnumerable<GameHostViewModel>>(hosts.Where(o => o.Location.Id == el.Id));
            }

            return Ok(locationFullViewModels);
        }
    }
}