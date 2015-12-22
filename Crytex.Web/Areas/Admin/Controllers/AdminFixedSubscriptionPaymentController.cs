using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Web.Models.JsonModels;
using System;
using System.Web.Http;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminFixedSubscriptionPaymentController : AdminCrytexController
    {
        private readonly IFixedSubscriptionPaymentService _paymentService;

        public AdminFixedSubscriptionPaymentController(IFixedSubscriptionPaymentService paymentService)
        {
            this._paymentService = paymentService;
        }

        [HttpGet]
        public IHttpActionResult Get(int pageNumber, int pageSize, DateTime? from = null, DateTime? to = null, string userId = null,
            string subscriptionVmId = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be grater or equal to 1");

            var searchParams = new FixedSubscriptionPaymentSearchParams
            {
                From = from,
                To = to,
                UserId = userId
            };

            Guid subGuid;
            if(subscriptionVmId != null)
            {
                if (!Guid.TryParse(subscriptionVmId, out subGuid))
                {
                    this.ModelState.AddModelError("id", "Invalid Guid format");
                    return BadRequest(ModelState);
                }
                searchParams.SubscriptionVmId = subGuid;
            }

            var pagedList = this._paymentService.GetPage(pageNumber, pageSize, searchParams);
            var pageModel = AutoMapper.Mapper.Map<PageModel<FixedSubscriptionPaymentViewModel>> (pagedList);

            return this.Ok(pageModel);
        }
    }
}