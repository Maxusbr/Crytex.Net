using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Web.Areas.Admin;
using Crytex.Web.Models.JsonModels;

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
        /// Получить конфигурацию готового физического сервера
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PhysicalServerViewModel))]
        [HttpGet]
        public IHttpActionResult GetReadyServer(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }
            var server = _serverService.GetReadyPhysicalServer(guid);
            var viewModel = AutoMapper.Mapper.Map<PhysicalServerViewModel>(server);
            return Ok(viewModel);
        }

        /// <summary>
        /// Получить конфигурацию физического сервера со списком доступных опций
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PhysicalServerViewModel))]
        [HttpGet]
        public IHttpActionResult GetAviableServer(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }
            var server = _serverService.GetAviablePhysicalServer(guid);
            var viewModel = AutoMapper.Mapper.Map<PhysicalServerViewModel>(server);
            return Ok(viewModel);
        }

        /// <summary>
        /// Купить физический сервер
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IHttpActionResult Buy(BoughtPhysicalServerViewModel model)
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
                UserId = model.UserId,
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

            return Ok(server);
        }

        /// <summary>
        /// Получить конфигурацию купленного физического сервера
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(BoughtPhysicalServerViewModel))]
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