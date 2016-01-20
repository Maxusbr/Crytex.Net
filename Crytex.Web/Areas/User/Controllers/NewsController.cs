using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Web.Models.JsonModels;
using Microsoft.Practices.Unity;

namespace Crytex.Web.Areas.User.Controllers
{
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
        /// <param name="filter"></param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<NewsViewModel>))]
        // GET: api/User/News
        public IHttpActionResult Get()
        {
            var news = this._newsService.GetAll();
            var viewModel = AutoMapper.Mapper.Map<IEnumerable<NewsViewModel>>(news);

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