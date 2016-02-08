using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Model.Enums;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminPhysicalServerController : AdminCrytexController
    {
        private readonly IPhysicalServerService _serverService;

        public AdminPhysicalServerController(IPhysicalServerService serverService)
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
        public IHttpActionResult Get(string id, PhysicalServerType type = PhysicalServerType.ReadyServer)
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
        /// Создать конфигурацию физического сервера
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Post([FromBody]PhysicalServerViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var param = AutoMapper.Mapper.Map<CreatePhysicalServerParam>(model);
            param.CalculatePrice = model.Price == 0;
            var server = _serverService.CreatePhysicalServer(param);

            return Ok(server);
        }

        /// <summary>
        /// Удалить конфигурацию физического сервера
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }
            _serverService.DeletePhysicalServer(guid);

            return Ok();
        }
    }
}