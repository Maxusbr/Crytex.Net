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


        private void UpdateServerStatus(Guid serverId, TypeChangeStatus status)
        {
            switch (status)
            {
                case TypeChangeStatus.Start:
                    _gameServerService.StartGameServer(serverId);
                    break;
                case TypeChangeStatus.PowerOff:
                    _gameServerService.PowerOffGameServer(serverId);
                    break;
                case TypeChangeStatus.Reload:
                    _gameServerService.ResetGameServer(serverId);
                    break;
                case TypeChangeStatus.Stop:
                    _gameServerService.StopGameServer(serverId);
                    break;
            }
        }

        /// <summary>
        /// Редактирование параметров игрового сервера
        /// </summary>
        [HttpPut]
        public IHttpActionResult UpdateGameServerConfiguration(GameServerConfigViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            Guid serverId;
            if (!Guid.TryParse(model.serverId, out serverId))
            {
                ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }
            var serviceOptions = Mapper.Map<GameServerConfigOptions>(model);

            switch (model.UpdateType)
            {
                case GameServerUpdateType.UpdateSettings:
                    if (string.IsNullOrEmpty(model.ServerName) && model.AutoProlongation == null)
                        return BadRequest("ServerName must be not empty");
                    _gameServerService.UpdateGameServer(serverId, serviceOptions);
                    break;
                case GameServerUpdateType.Prolongation:
                    if(model.ProlongatePeriod <= 0) return BadRequest("MonthCount must be greater than 0");
                    _gameServerService.UpdateGameServer(serverId, serviceOptions);
                    break;
                case GameServerUpdateType.UpdateSlotCount:
                    if (model.SlotCount == null || model.SlotCount <= 0)
                    {
                        return BadRequest("SlotCount must be > 0");
                    }
                    _gameServerService.UpdateGameServer(serverId, serviceOptions);
                    break;
                default:
                    return BadRequest("Invalid UpdateType");
            }
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult UpdateServerStatus(GameServerChangeStatusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UpdateServerStatus(model.ServerId.Value, model.ChangeStatusType.Value);

            return Ok();
        }
    }
}