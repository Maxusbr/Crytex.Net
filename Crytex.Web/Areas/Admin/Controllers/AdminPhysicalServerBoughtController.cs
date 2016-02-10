using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Model.Enums;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminPhysicalServerBoughtController : AdminCrytexController
    {
        private readonly IPhysicalServerService _serverService;

        public AdminPhysicalServerBoughtController(IPhysicalServerService serverService)
        {
            _serverService = serverService;
        }

        /// <summary>
        /// Список купленных серверов
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [ResponseType(typeof(PageModel<BoughtPhysicalServerViewModel>))]
        [HttpGet]
        public IHttpActionResult GetBoughtServers(int pageNumber, int pageSize)
        {
            var servers = _serverService.GetPageBoughtPhysicalServer(pageNumber, pageSize);
            var viewModel = AutoMapper.Mapper.Map<PageModel<BoughtPhysicalServerViewModel>>(servers);
            return Ok(viewModel);
        }

        /// <summary>
        /// Получить конфигурацию купленного физического сервера
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PhysicalServerViewModel))]
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

        /// <summary>
        /// Изменить статус купленного физического сервера
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult ChangeStatusServer(ChangePhysicalServerViewModel model)
        {
            Guid guid;
            if (!Guid.TryParse(model.ServerId, out guid))
            {
                ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }
            _serverService.UpdateBoughtPhysicalServerState(new PhysicalServerStateParams
            {
                ServerId = guid,
                State = (BoughtPhysicalServerStatus)model.Status,
                AdminMessage = model.Message,
                AutoProlongation = model.AutoProlongation
            });

            return Ok();
        }
    }
}