using System.Web.Http;
using AutoMapper;
using Crytex.Model.Enums;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.User.Controllers
{
    [AllowAnonymous]
    public class GameController : UserCrytexController
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var game = _gameService.GetById(id);

            var gameModel = Mapper.Map<GameViewModel>(game);

            return Ok(gameModel);
        }

        /// <summary>
        /// Вывод  GameViewModel с пагинацией.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="familyGame"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(int pageNumber, int pageSize, GameFamily familyGame)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be equal or grater than 1");

            var page = _gameService.GetPage(pageNumber, pageSize, familyGame);
            var pageModel = Mapper.Map<PageModel<GameViewModel>>(page);

            return this.Ok(pageModel);
        }
    }
}