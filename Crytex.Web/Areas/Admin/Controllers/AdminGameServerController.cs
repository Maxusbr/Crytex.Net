using System;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System.Web.Http;
using AutoMapper;
using Crytex.Model.Enums;
using Crytex.Model.Models;
using Crytex.Service.Model;

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
        public IHttpActionResult Get(int pageNumber, int pageSize, string userId = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be equal or grater than 1");

            var page = this._gameServerService.GetPage(pageNumber, pageSize, userId);
            var pageModel = AutoMapper.Mapper.Map<PageModel<GameServerViewModel>>(page);

            return this.Ok(pageModel);
        }

        /// <summary>
        /// Обновление статуса GameServer
        /// </summary>
        [HttpPost]
        public IHttpActionResult UpdateMachineStatus([FromBody]UpdateGameServerOptions model)
        {
            Guid guid;
            if (!Guid.TryParse(model.serverId, out guid))
                return this.BadRequest("Invalid Guid format");
            switch (model.Status)
            {
                case TypeChangeStatus.Start:
                    _gameServerService.StartGameServer(guid);
                    break;
                case TypeChangeStatus.PowerOff:
                    _gameServerService.PowerOffGameServer(guid);
                    break;
                case TypeChangeStatus.Reload:
                    _gameServerService.ResetGameServer(guid);
                    break;
                case TypeChangeStatus.Stop:
                    _gameServerService.StopGameServer(guid);
                    break;
                default:
                    return BadRequest("Invalid status");
            }

            return Ok();
        }

        /// <summary>
        /// Редактирование параметров игрового сервера
        /// </summary>
        [HttpPost]
        public IHttpActionResult UpdateGameServerConfiguration(GameServerConfigViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var serviceOptions = Mapper.Map<GameServerConfigOptions>(model);
            serviceOptions.UpdateType = GameServerUpdateType.Configuration;
            _gameServerService.UpdateGameServer(model.ServerId, serviceOptions);

            return Ok();
        }

        /// <summary>
        /// Включение автооплаты
        /// </summary>
        [HttpPost]
        public IHttpActionResult EnableAutoProlongationGameServer(GameServerConfigViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var serviceOptions = new GameServerConfigOptions
            {
                AutoProlongation = model.AutoProlongation,
                UpdateType = GameServerUpdateType.Configuration
            };
            _gameServerService.UpdateGameServer(model.ServerId, serviceOptions);

            return Ok();
        }
    }
}