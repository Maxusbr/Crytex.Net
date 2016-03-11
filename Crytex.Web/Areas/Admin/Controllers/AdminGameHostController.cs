using System;
using System.Web.Http;
using AutoMapper;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminGameHostController : AdminCrytexController
    {
        private readonly IGameHostService _gameHostService;

        public AdminGameHostController(IGameHostService gameHostService)
        {
            _gameHostService = gameHostService;
        }

        /// <summary>
        /// Вывод GameHostViewModel
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be equal or grater than 1");

            var page = _gameHostService.GetPage(pageNumber, pageSize);
            var pageModel = AutoMapper.Mapper.Map<PageModel<GameHostViewModel>>(page);

            return this.Ok(pageModel);
        }

        [HttpPost]
        public IHttpActionResult Post(GameHostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var options = Mapper.Map<GameHostCreateOptions>(model);
            var host = _gameHostService.Create(options);

            return Ok(new {id = host.Id});
        }

        /// <summary>
        /// Обновление GameHost
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult Put(GameHostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var options = Mapper.Map<GameHostCreateOptions>(model);
            _gameHostService.Update(model.Id, options);

            return Ok();
        }

        /// <summary>
        /// Удаление GameHost
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult Delete(Int32 id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _gameHostService.Delete(id);

            return Ok();
        }
    }
}