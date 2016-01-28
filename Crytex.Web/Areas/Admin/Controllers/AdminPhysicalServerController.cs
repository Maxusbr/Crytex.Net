using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Crytex.Model.Enums;
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
        public IHttpActionResult Get(int pageNumber, int pageSize)
        {
            var servers = _serverService.GetPagePhysicalServer(pageNumber, pageSize);
            var viewModel = AutoMapper.Mapper.Map<PageModel<PhysicalServerViewModel>>(servers);
            return Ok(viewModel);
        }

        /// <summary>
        /// Список опций
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [ResponseType(typeof(PageModel<PhysicalServerOptionViewModel>))]
        public IHttpActionResult GetOptions(int pageNumber, int pageSize)
        {
            var options = _serverService.GetPagePhysicalServerOption(pageNumber, pageSize);
            var viewModel = AutoMapper.Mapper.Map<PageModel<PhysicalServerOptionViewModel>>(options);
            return Ok(viewModel);
        }

        /// <summary>
        /// Список купленных серверов
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [ResponseType(typeof(PageModel<BoughtPhysicalServerViewModel>))]
        public IHttpActionResult GetBoughtServers(int pageNumber, int pageSize)
        {
            var servers = _serverService.GetPageBoughtPhysicalServer(pageNumber, pageSize);
            var viewModel = AutoMapper.Mapper.Map<PageModel<BoughtPhysicalServerViewModel>>(servers);
            return Ok(viewModel);
        }

        /// <summary>
        /// Получить конфигурацию готового физического сервера
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PhysicalServerViewModel))]
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
        /// Получить конфигурацию купленного физического сервера
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(PhysicalServerViewModel))]
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
        /// Создать конфигурацию физического сервера
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
        /// Создать опцию для физического сервера
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IHttpActionResult Post([FromBody]PhysicalServerOptionViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var param = new PhysicalServerOptionsParams
            {
                Name = model.Name, Description = model.Description, Price = model.Price, Type = model.Type
            };
            var server = _serverService.CreateOrUpdateOption(param);

            return Ok(server);
        }

        /// <summary>
        /// Создать опции для физического сервера
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IHttpActionResult Post([FromBody]IEnumerable<PhysicalServerOptionViewModel> options)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var param = options.Select(opt => new PhysicalServerOptionsParams
            {
                Name = opt.Name, Description = opt.Description, Price = opt.Price, Type = opt.Type
            });
            _serverService.CreateOrUpdateOptions(param);

            return Ok();
        }

        /// <summary>
        /// Добавить доступные опции для физического сервера
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="options"></param>
        /// <param name="replaceAll"></param>
        /// <returns></returns>
        public IHttpActionResult ChangeOptionsAviable(string serverId, [FromBody]IEnumerable<PhysicalServerOptionViewModel> options, bool replaceAll)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Guid guid;
            if (!Guid.TryParse(serverId, out guid))
            {
                ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }
            var parameters = new PhysicalServerOptionsAviableParams {ServerId = guid, ReplaceAll = replaceAll};
            var optionsnAviable = new List<OptionAviable>();
            foreach (var opt in options)
            {
                Guid optguid;
                if (!Guid.TryParse(opt.Id, out optguid))
                {
                    ModelState.AddModelError("id", "Invalid Guid format");
                    return BadRequest(ModelState);
                }
                var ids = new OptionAviable {OptionId = optguid, IsDefault = opt.IsDefault};
                optionsnAviable.Add(ids);
            }
            parameters.Options = optionsnAviable;
            _serverService.UpdateOptionsAviable(parameters);

            return Ok();
        }

        /// <summary>
        /// Удалить конфигурацию физического сервера
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public IHttpActionResult DaeletePhysicalServer(string serverId)
        {
            Guid guid;
            if (!Guid.TryParse(serverId, out guid))
            {
                ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }
            _serverService.DeletePhysicalServer(guid);

            return Ok();
        }

        /// <summary>
        /// Удалить опцию физического сервера
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public IHttpActionResult DaeletePhysicalServerOption(string serverId)
        {
            Guid guid;
            if (!Guid.TryParse(serverId, out guid))
            {
                ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }
            _serverService.DeletePhysicalServerOption(guid);

            return Ok();
        }

        /// <summary>
        /// Изменить статус купленного физического сервера
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public IHttpActionResult ChangeStatusServer(string serverId, int status)
        {
            Guid guid;
            if (!Guid.TryParse(serverId, out guid))
            {
                ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }
            _serverService.UpdateBoughtPhysicalServerState(guid, (BoughtPhysicalServerStatus)status);

            return Ok();
        }
    }
}