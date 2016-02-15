using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminDhcpServerController : AdminCrytexController
    {
        private readonly IDhcpServerService _dhspServerService;

        public AdminDhcpServerController(IDhcpServerService dhspServerService)
        {
            _dhspServerService = dhspServerService;
        }

        /// <summary>
        /// Получить DHCP сервер
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }

            var server = _dhspServerService.GetDhcpServerById(guid);
            var model = AutoMapper.Mapper.Map<DhcpServerView>(server);
            return Ok(model);
        }

        /// <summary>
        /// Список DHCP серверов
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be equal or grater than 1");

            var page = _dhspServerService.GetPageDhcpServer(pageNumber, pageSize);
            var pageModel = AutoMapper.Mapper.Map<PageModel<DhcpServerView>>(page);

            return this.Ok(pageModel);
        }

        /// <summary>
        /// Создать DHCP сервер
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Post(DhcpServerView model)
        {
            var option = AutoMapper.Mapper.Map<DhcpServerOption>(model);
            var server = _dhspServerService.CreateDhcpServer(option);
            return Ok(new { id = server.Id });
        }

        /// <summary>
        /// Изменить DHCP сервер
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult Put(DhcpServerView model)
        {
            Guid guid;
            if (!Guid.TryParse(model.Id, out guid))
            {
                ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }

            var option = AutoMapper.Mapper.Map<DhcpServerOption>(model);
            option.Id = guid;
            _dhspServerService.UpdateDhcpServer(option);
            return Ok();
        }

        /// <summary>
        /// Удалить DHCP сервер
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

            _dhspServerService.DeleteDhcpServer(guid);
            return Ok();
        }
    }
}