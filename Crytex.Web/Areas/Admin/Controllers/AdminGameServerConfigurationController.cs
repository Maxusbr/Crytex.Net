using System;
using System.Collections.Generic;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System.Web.Http;
using AutoMapper;
using Crytex.Model.Enums;
using Crytex.Model.Models;
using Crytex.Service.Model;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminGameServerConfigurationController : AdminCrytexController
    {
        private readonly IGameServerService _gameServerService;

        public AdminGameServerConfigurationController(IGameServerService gameServerService)
        {
            this._gameServerService = gameServerService;
        }

        /// <summary>
        /// Получить список конфигураций игровых серверов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetGameServerConfig()
        {

            var configs = _gameServerService.GetGameServerConfigurations();
            var model = Mapper.Map<IEnumerable<GameServerConfigurationView>>(configs);

            return Ok(model);
        }

        /// <summary>
        /// Создать конфигурацию игрового сервера
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Post([FromBody]GameServerConfigurationView model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var config = Mapper.Map<GameServerConfiguration>(model);
            config = _gameServerService.CreateGameServerConfiguration(config);

            return Ok(new { id = config.Id });
        }

        /// <summary>
        /// Изменить конфигурацию игрового сервера
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateGameServerConfig(GameServerConfigurationView model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var config = Mapper.Map<GameServerConfiguration>(model);
            _gameServerService.UpdateGameServerConfiguration(config);

            return Ok();
        }
    }
}