using AutoMapper;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System;
using System.Web.Http;

namespace Crytex.Web.Areas.User.Controllers
{
    public class WebHostingController : UserCrytexController
    {
        private readonly IWebHostingService _webHostingService;

        public WebHostingController(IWebHostingService webHostingService)
        {
            this._webHostingService = webHostingService;
        }

        [HttpPost]
        public IHttpActionResult BuyWebHosting(BuyWebHostingParams buyParams)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var userId = this.CrytexContext.UserInfoProvider.GetUserId();
            var webHostingEntity = this._webHostingService.BuyNewHosting(buyParams.WebHostingTariffId, userId);
            var model = Mapper.Map<WebHostingViewModel>(webHostingEntity);

            return this.Ok(model);
        }
    }
}