using System.Collections.Generic;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System.Web.Http;
using AutoMapper;
using Crytex.Model.Models.GameServers;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminGameServerTariffController : AdminCrytexController
    {
        private readonly IGameServerService _gameServerService;

        public AdminGameServerTariffController(IGameServerService gameServerService)
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

            var tariffs = _gameServerService.GetGameServerTariffs();
            var model = Mapper.Map<IEnumerable<GameServerTariffView>>(tariffs);

            return Ok(model);
        }

        /// <summary>
        /// Создать конфигурацию игрового сервера
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Post([FromBody]GameServerTariffView model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var config = Mapper.Map<GameServerTariff>(model);
            config = _gameServerService.CreateGameServerTariff(config);

            return Ok(new { id = config.Id });
        }

        /// <summary>
        /// Изменить конфигурацию игрового сервера
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateGameServerTariff(GameServerTariffView model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var config = Mapper.Map<GameServerTariff>(model);
            _gameServerService.UpdateGameServerTariff(config);

            return Ok();
        }
    }
}