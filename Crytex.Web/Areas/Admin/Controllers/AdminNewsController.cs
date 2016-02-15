using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminNewsController : AdminCrytexController
    {
        private readonly INewsService _newsService;

        public AdminNewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        /// <summary>
        /// Получение всех News
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// GET: api/Admin/AdminNews
        [ResponseType(typeof(PageModel<NewsViewModel>))]
        public IHttpActionResult Get(int pageNumber, int pageSize)
        {
            var news = this._newsService.GetPage(pageNumber, pageSize);
            var viewModel = AutoMapper.Mapper.Map<PageModel<NewsViewModel>>(news);

            return Ok(viewModel);
        }

        /// <summary>
        /// Получение News по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Admin/AdminNews/5
        [ResponseType(typeof(NewsViewModel))]
        public IHttpActionResult Get(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                this.ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }

            var news = this._newsService.GetNewsById(guid);
            var viewModel = AutoMapper.Mapper.Map<NewsViewModel>(news);

            return Ok(viewModel);
        }

        /// <summary>
        /// Создание новой News
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: api/Admin/AdminNews
        public IHttpActionResult Post([FromBody]NewsViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = this.CrytexContext.UserInfoProvider.GetUserId();
            var news = AutoMapper.Mapper.Map<News>(model);
            news.UserId = userId;
            news.CreateTime = DateTime.UtcNow;
            var newnews = this._newsService.CreateNews(news);

            return Ok(newnews);
        }

        /// <summary>
        /// Обновление News
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        // PUT: api/Admin/AdminNews/5
        public IHttpActionResult Put(string id, [FromBody]NewsViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                this.ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }
            var userId = this.CrytexContext.UserInfoProvider.GetUserId();
            var news = AutoMapper.Mapper.Map<News>(model);
            news.UserId = userId;
            news.CreateTime = DateTime.UtcNow;
            news.Id = guid;
            this._newsService.UpdateNews(news);

            return Ok();
        }

        /// <summary>
        /// Удаление News по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Admin/AdminNews/5
        public IHttpActionResult Delete(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                this.ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }

            this._newsService.DeleteNewsById(guid);
            return Ok();
        }
    }
}