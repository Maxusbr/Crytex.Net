using System;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Web.Models.JsonModels;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Web.Http;
using Crytex.Model.Models.Biling;
using PagedList;

namespace Crytex.Web.Areas.User.Controllers
{
    public class FixedSubscriptionPaymentController : UserCrytexController
    {
        private readonly IFixedSubscriptionPaymentService _paymentService;

        public FixedSubscriptionPaymentController([Dependency("Secured")]IFixedSubscriptionPaymentService paymentService)
        {
            this._paymentService = paymentService;
        }

        [HttpGet]
        public IHttpActionResult Get(int pageNumber, int pageSize, [FromUri] FixedSubscriptionPaymentSearchParamViewModel searchParams = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be grater or equal to 1");

            IPagedList<FixedSubscriptionPayment> fixedSubscriptionPayments = new PagedList<FixedSubscriptionPayment>(new List<FixedSubscriptionPayment>(), pageNumber, pageSize);

            var fixedSubscriptionPaymentParams = AutoMapper.Mapper.Map<FixedSubscriptionPaymentSearchParams>(searchParams);

            fixedSubscriptionPayments = _paymentService.GetPage(pageNumber, pageSize, fixedSubscriptionPaymentParams);

            var pageModel = AutoMapper.Mapper.Map<PageModel<FixedSubscriptionPaymentViewModel>>(fixedSubscriptionPayments);

            return this.Ok(pageModel);
        }
    }
}