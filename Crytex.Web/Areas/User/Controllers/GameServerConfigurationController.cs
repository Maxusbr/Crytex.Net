using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Crytex.Service.Model;
using AutoMapper;
using Crytex.Model.Enums;

namespace Crytex.Web.Areas.User.Controllers
{
    public class GameServerConfigurationController : UserCrytexController
    {
        private readonly IGameServerService _gameServerService;

        public GameServerConfigurationController([Dependency("Secured")]IGameServerService gameServerService)
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
            var model = AutoMapper.Mapper.Map<IEnumerable<GameServerConfigurationView>>(configs);

            return Ok(model);
        }

        


    }
}
