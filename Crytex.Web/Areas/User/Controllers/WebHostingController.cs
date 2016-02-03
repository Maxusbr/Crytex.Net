using AutoMapper;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Web.Models.JsonModels;
using Microsoft.Practices.Unity;
using System;
using System.Web.Http;

namespace Crytex.Web.Areas.User.Controllers
{
    public class WebHostingController : UserCrytexController
    {
        private readonly IWebHostingService _webHostingService;
        private readonly IHostedWebApplicationService _webApplicationService;

        public WebHostingController([Dependency("Secured")]IWebHostingService webHostingService,
            [Dependency("Secured")]IHostedWebApplicationService webApplicationService)
        {
            this._webHostingService = webHostingService;
            this._webApplicationService = webApplicationService;
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

        [HttpPost]
        public IHttpActionResult ProlongateHosting(WebHostingProlongateOptionsModel prolongateOptionsModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            this._webHostingService.ProlongateWebHosting(prolongateOptionsModel.WebHostingId, prolongateOptionsModel.MonthCount);

            return this.Ok();
        }

        [HttpPost]
        public IHttpActionResult StartWebApplication(Guid appId)
        {
            this._webApplicationService.StartApplication(appId);

            return this.Ok();
        }

        [HttpPost]
        public IHttpActionResult StopWebApplication(Guid appId)
        {
            this._webApplicationService.StopApplication(appId);

            return this.Ok();
        }

        [HttpPost]
        public IHttpActionResult RestartWebApplication(Guid appId)
        {
            this._webApplicationService.RestartApplication(appId);

            return this.Ok();
        }

        [HttpPut]
        public IHttpActionResult Put([FromUri]Guid id, [FromBody]WebHostingUpdateModel updateModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            this._webHostingService.UpdateWebHosting(id, updateModel.Name, updateModel.AutoProlongation);

            return this.Ok();
        }
    }
}