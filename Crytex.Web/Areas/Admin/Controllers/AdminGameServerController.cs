using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System.Web.Http;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminGameServerController : AdminCrytexController
    {
        private readonly IGameServerService _gameServerService;

        public AdminGameServerController(IGameServerService gameServerService)
        {
            this._gameServerService = gameServerService;
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var server = this._gameServerService.GetById(id);
            var model = AutoMapper.Mapper.Map<GameServerViewModel>(server);

            return this.Ok(model);
        }

        [HttpGet]
        public IHttpActionResult Get(int pageNumber, int pageSize, string userId = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be equal or grater than 1");

            var page = this._gameServerService.GetPage(pageNumber, pageSize, userId);
            var pageModel = AutoMapper.Mapper.Map<PageModel<GameServerViewModel>>(page);

            return this.Ok(pageModel);
        }
    }
}