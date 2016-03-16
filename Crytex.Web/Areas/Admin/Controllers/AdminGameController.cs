using System;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using Crytex.Model.Enums;
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

        /// <summary>
        /// Обновление Game
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult Put(GameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var game = Mapper.Map<Game>(model);
            _gameService.Update(game);

            return Ok();
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

        /// <summary>
        /// Удаляем игру по полученному Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult Delete(Int32 Id)
        {
            _gameService.Delete(Id);

            return Ok();
        }
    }
}