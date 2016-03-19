using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Web.Http;

namespace Crytex.Web.Areas.User.Controllers
{
    public class GameServerTariffController : UserCrytexController
    {
        private readonly IGameServerService _gameServerService;

        public GameServerTariffController([Dependency("Secured")]IGameServerService gameServerService)
        {
            this._gameServerService = gameServerService;
        }

        /// <summary>
        /// Получить список конфигураций игровых серверов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetGameServerTariff()
        {
            var configs = _gameServerService.GetGameServerTariffs();
            var model = AutoMapper.Mapper.Map<IEnumerable<GameServerTariffView>>(configs);

            return Ok(model);
        }

        


    }
}
