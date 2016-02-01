using AutoMapper;
using Crytex.Model.Models.Biling;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using Microsoft.Practices.Unity;
using System;
using System.Web.Http;

namespace Crytex.Web.Areas.User.Controllers
{
    public class WebHostingPaymentController : UserCrytexController
    {
        private readonly IWebHostingService _webHostingService;

        public WebHostingPaymentController([Dependency("Secured")]IWebHostingService webHostingService)
        {
            this._webHostingService = webHostingService;
        }

        public IHttpActionResult Get(int pageNumber, int pageSize, Guid? webHostingId = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("PageNumber and PageSize must be grater than 1");
            }

            var userId = this.CrytexContext.UserInfoProvider.GetUserId();
            //var webHostingGuid = new Guid(webHostingId);
            var page = this._webHostingService.GetWebHostingPaymentsPaged(pageNumber, pageSize, userId, webHostingId);
            var pageModel = Mapper.Map<PageModel<WebHostingPaymentViewModel>>(page);

            return this.Ok(pageModel);
        }
    }
}