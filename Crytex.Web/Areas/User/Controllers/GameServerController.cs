using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using Microsoft.Practices.Unity;
using System;
using System.Web.Http;
using Crytex.Service.Model;

namespace Crytex.Web.Areas.User.Controllers
{
    public class GameServerController : UserCrytexController
    {
        private readonly IGameServerService _gameServerService;

        public GameServerController([Dependency("Secured")]IGameServerService gameServerService)
        {
            this._gameServerService = gameServerService;
        }

        [HttpGet]
        public IHttpActionResult Get(String id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                this.ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }

            var server = this._gameServerService.GetById(guid);
            var model = AutoMapper.Mapper.Map<GameServerViewModel>(server);

            return this.Ok(model);
        }

        [HttpGet]
        public IHttpActionResult Get(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be equal or grater than 1");

            var userId = this.CrytexContext.UserInfoProvider.GetUserId();
            var page = this._gameServerService.GetPage(pageNumber, pageSize, userId);
            var pageModel = AutoMapper.Mapper.Map<PageModel<GameServerViewModel>>(page);

            return this.Ok(pageModel);
        }

        [HttpPost]
        public IHttpActionResult Post(GameServerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var server = AutoMapper.Mapper.Map<GameServer>(model);
            server.UserId = this.CrytexContext.UserInfoProvider.GetUserId();

            server = this._gameServerService.CreateServer(server);

            return this.Ok(new { id = server.Id});
        }

        /// <summary>
        /// покупка игровых машин
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Buy(GameServerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var server = AutoMapper.Mapper.Map<GameServer>(model);
            var options = AutoMapper.Mapper.Map<BuyGameServerOption>(model);
            server.UserId = this.CrytexContext.UserInfoProvider.GetUserId();

            server = this._gameServerService.BuyGameServer(server, options);

            return this.Ok(new { id = server.Id });
        }
    }
}
