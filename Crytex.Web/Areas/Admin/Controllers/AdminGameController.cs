using System.Web.Http;
using AutoMapper;
using Crytex.Model.Models.GameServers;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminGameController : AdminCrytexController
    {
        private readonly IGameService _gameService;

        public AdminGameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost]
        public IHttpActionResult Post(GameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var game = Mapper.Map<Game>(model);
            game = _gameService.Create(game);

            return Ok(new {id = game.Id});
        }
    }
}