using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Web.Areas.Admin;
using Crytex.Web.Models.JsonModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Crytex.Web.Areas.User.Controllers
{
    public class PhysicalServerController : AdminCrytexController
    {
        private readonly IPhysicalServerService _serverService;

        public PhysicalServerController(IPhysicalServerService serverService)
        {
            _serverService = serverService;
        }

        /// <summary>
        /// Получить список серверов
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [ResponseType(typeof(PageModel<PhysicalServerViewModel>))]
        [HttpGet]
        public IHttpActionResult Get(int pageNumber, int pageSize)
        {
            var servers = _serverService.GetPagePhysicalServer(pageNumber, pageSize);
            var viewModel = AutoMapper.Mapper.Map<PageModel<PhysicalServerViewModel>>(servers);
            return Ok(viewModel);
        }

        /// <summary>
        /// Получить конфигурацию физического сервера
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [ResponseType(typeof(PhysicalServerViewModel))]
        [HttpGet]
        public IHttpActionResult GetServer(string id, PhysicalServerType type)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }
            PhysicalServer server = null;
            switch (type)
            {
                case PhysicalServerType.ReadyServer:
                    server = _serverService.GetReadyPhysicalServer(guid);
                    break;
                case PhysicalServerType.AviableServer:
                    server = _serverService.GetAviablePhysicalServer(guid);
                    break;
                default:
                    ModelState.AddModelError("type", "Invalid Physical Server type");
                    return BadRequest(ModelState);
            }

            if (server == null)
            {
                ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }
            var viewModel = AutoMapper.Mapper.Map<PhysicalServerViewModel>(server);
            return Ok(viewModel);
        }


        /// <summary>
        /// Купить физический сервер
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Post([FromBody]BoughtPhysicalServerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Guid serverId;
            if (!Guid.TryParse(model.PhysicalServerId, out serverId))
            {
                ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }

            var parameters = new BuyPhysicalServerParam
            {
                PhysicalServerId = serverId,
                UserId = CrytexContext.UserInfoProvider.GetUserId(),
                CountMonth = model.CountMonth,
                DiscountPrice = model.DiscountPrice,
                AutoProlongation = model.AutoProlongation
            };
            if (model.CreateDate != null)
                parameters.CreateDate = model.CreateDate;
            var options = new List<Guid>();
            foreach (var opt in model.Options)
            {
                Guid optId;
                if (!Guid.TryParse(opt.Id, out optId))
                {
                    ModelState.AddModelError("id", "Invalid Guid format");
                    return BadRequest(ModelState);
                }
                options.Add(optId);
            }
            parameters.OptionIds = options;
            var server = _serverService.BuyPhysicalServer(parameters);

            return Ok(new { id = server.Id });
        }

        /// <summary>
        /// Получить конфигурацию купленного физического сервера
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(BoughtPhysicalServerViewModel))]
        [HttpGet]
        public IHttpActionResult GetBoughtServer(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }
            var server = _serverService.GetBoughtPhysicalServer(guid);
            var viewModel = AutoMapper.Mapper.Map<BoughtPhysicalServerViewModel>(server);
            return Ok(viewModel);
        }
    }
}