using System.Web.Http;
using AutoMapper;
using Crytex.Model.Models.GameServers;
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
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult Put(int id, GameHostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var options = Mapper.Map<GameHostCreateOptions>(model);
            _gameHostService.Update(id, options);

            return Ok();
        }
    }
}