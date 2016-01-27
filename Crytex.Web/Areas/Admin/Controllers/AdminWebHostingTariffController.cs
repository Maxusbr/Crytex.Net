using AutoMapper;
using Crytex.Model.Models.WebHosting;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System.Web.Http;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminWebHostingTariffController : AdminCrytexController
    {
        private readonly IWebHostingTariffService _webHostingTariffService;

        public AdminWebHostingTariffController(IWebHostingTariffService webHostingTariffService)
        {
            this._webHostingTariffService = webHostingTariffService;
        }

        public IHttpActionResult Get(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("PageNumber and PageSize must be grater or equal to 1");
            }

            var page = this._webHostingTariffService.GetPage(pageNumber, pageSize);
            var pageModel = Mapper.Map<PageModel<WebHostingTariffViewModel>>(page);

            return this.Ok(pageModel);
        }

        public IHttpActionResult Post(WebHostingTariffViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var tariff = Mapper.Map<WebHostingTariff>(model);
            tariff = this._webHostingTariffService.Create(tariff);

            return this.Ok(tariff);
        }
    }
}