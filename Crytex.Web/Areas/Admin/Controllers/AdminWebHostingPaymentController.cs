using AutoMapper;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System;
using System.Web.Http;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminWebHostingPaymentController : AdminCrytexController
    {
        private readonly IWebHostingService _webHostingService;

        public AdminWebHostingPaymentController(IWebHostingService webHostingService)
        {
            this._webHostingService = webHostingService;
        }

        public IHttpActionResult Get(int pageNumber, int pageSize, string userId = null, Guid? webHostingId = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("PageNumber and PageSize must be grater than 1");
            }

            //var webHostingGuid = new Guid(webHostingId);
            var page = this._webHostingService.GetWebHostingPaymentsPaged(pageNumber, pageSize, userId, webHostingId);
            var pageModel = Mapper.Map<PageModel<WebHostingPaymentViewModel>>(page);

            return this.Ok(pageModel);
        }
    }
}