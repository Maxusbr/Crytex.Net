using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Web.Models.JsonModels;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Model.Models.Biling;
using PagedList;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminFixedSubscriptionPaymentController : AdminCrytexController
    {
        private readonly IFixedSubscriptionPaymentService _paymentService;

        public AdminFixedSubscriptionPaymentController(IFixedSubscriptionPaymentService paymentService)
        {
            this._paymentService = paymentService;
        }

        /// <summary>
        /// Получение списка FixedSubscriptionPaymentViewModel
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        // GET: Admin/AdminFixedSubscriptionPayment
        [ResponseType(typeof(PageModel<TaskV2ViewModel>))]
        [HttpGet]
        public IHttpActionResult Get(int pageNumber, int pageSize, [FromUri] AdminFixedSubscriptionPaymentSearchParamViewModel searchParams = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be grater or equal to 1");

            IPagedList<FixedSubscriptionPayment> fixedSubscriptionPayments = new PagedList<FixedSubscriptionPayment>(new List<FixedSubscriptionPayment>(), pageNumber, pageSize);

            if (searchParams != null)
            {
                var fixedSubscriptionPaymentParams = AutoMapper.Mapper.Map<FixedSubscriptionPaymentSearchParams>(searchParams);
                fixedSubscriptionPayments = _paymentService.GetPage(pageNumber, pageSize, fixedSubscriptionPaymentParams);
            }
            else
            {
                fixedSubscriptionPayments = _paymentService.GetPage(pageNumber, pageSize);
            }

            var pageModel = AutoMapper.Mapper.Map<PageModel<FixedSubscriptionPaymentViewModel>> (fixedSubscriptionPayments);

            return this.Ok(pageModel);
        }
    }
}