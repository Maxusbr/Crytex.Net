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


        private bool UpdateMachineStatus(Guid serverId, TypeChangeStatus? status)
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
                default:
                    return false;
            }
            return true;
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
                case GameServerUpdateType.UpdateName:
                    if (string.IsNullOrEmpty(model.ServerName)) return BadRequest("ServerName must be not empty");
                    _gameServerService.UpdateGameServer(serverId, serviceOptions);
                    break;
                case GameServerUpdateType.Prolongation:
                    if(model.MonthCount <= 0) return BadRequest("MonthCount must be greater than 0");
                    _gameServerService.UpdateGameServer(serverId, serviceOptions);
                    break;
                case GameServerUpdateType.EnableAutoProlongation:
                    if (model.AutoProlongation == null) return BadRequest("AutoProlongation must have value");
                    _gameServerService.UpdateGameServer(serverId, serviceOptions);
                    break;
                case GameServerUpdateType.State:
                    if(!UpdateMachineStatus(serverId, model.Status)) return BadRequest("Invalid status");
                    break;
                default:
                    return BadRequest("Invalid UpdateType");
            }
            return Ok();
        }


    }
}