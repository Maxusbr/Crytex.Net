using AutoMapper;
using Crytex.Service.IService;
using Crytex.Service.Model;
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
        public IHttpActionResult BuyWebHosting(BuyWebHostingParamsModel buyParamsModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var buyParams = Mapper.Map<BuyWebHostingParams>(buyParamsModel);
            var userId = this.CrytexContext.UserInfoProvider.GetUserId();
            buyParams.UserId = userId;
            if(buyParamsModel.MonthCount <= 0)
            {
                buyParams.MonthCount = 1;
            }
            var webHostingEntity = this._webHostingService.BuyNewHosting(buyParams);
            var outModel = Mapper.Map<WebHostingViewModel>(webHostingEntity);

            return this.Ok(outModel);
        }
    }
}