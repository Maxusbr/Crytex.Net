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
using Crytex.Model.Models.GameServers;

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

        /// <summary>
        /// покупка игровых машин
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Post([FromBody]GameServerBuyOptionsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var options = AutoMapper.Mapper.Map<BuyGameServerOption>(model);
            var userId = this.CrytexContext.UserInfoProvider.GetUserId();
            options.UserId = userId;

            var server = this._gameServerService.BuyGameServer(options);

            return this.Ok(new { id = server.Id });
        }

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
                    if (model.MonthCount <= 0) return BadRequest("MonthCount must be greater than 0");
                    _gameServerService.UpdateGameServer(serverId, serviceOptions);
                    break;
                case GameServerUpdateType.EnableAutoProlongation:
                    if (model.AutoProlongation == null) return BadRequest("AutoProlongation must have value");
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
    }
}
