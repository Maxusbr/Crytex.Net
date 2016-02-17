using System;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.User.Controllers
{
    [AllowAnonymous]
    public class NewsController : UserCrytexController
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            this._newsService = newsService;
        }

        /// <summary>
        /// Получение списка Payment
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [ResponseType(typeof(PageModel<NewsViewModel>))]
        // GET: api/User/News
        public IHttpActionResult Get(int pageNumber, int pageSize)
        {
            var news = this._newsService.GetPage(pageNumber, pageSize);
            var viewModel = AutoMapper.Mapper.Map<PageModel<NewsViewModel>>(news);

            return Ok(viewModel);
        }

        // GET: api/User/News/5
        public IHttpActionResult Get(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                this.ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }

            var order = this._newsService.GetNewsById(guid);
            var model = AutoMapper.Mapper.Map<NewsViewModel>(order);

            return Ok(model);
        }
    }
}