using System.Web.Http;
using AutoMapper;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminGameHostController : AdminCrytexController
    {
        private readonly IGameHostService _gameHostService;

        public AdminGameHostController(IGameHostService gameHostService)
        {
            _gameHostService = gameHostService;
        }

        [HttpPost]
        public IHttpActionResult Post(GameHostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var options = Mapper.Map<GameHostCreateOptions>(model);
            var host = _gameHostService.Create(options);

            return Ok(new {id = host.Id});
        }
    }
}