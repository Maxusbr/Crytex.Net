using System;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System.Web.Http;
using Crytex.Data.IRepository;
using Crytex.Model.Models.Biling;
using Crytex.Service.Model;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminUsageSubscriptionPaymentController : AdminCrytexController
    {
        private readonly ISubscriptionVmService _subscriptionVmService;

        public AdminUsageSubscriptionPaymentController(ISubscriptionVmService subscriptionVmService)
        {
            this._subscriptionVmService = subscriptionVmService;
        }

        /// <summary>
        /// Получение всех SubscriptionPayment
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="userId"></param>
        ///<param name="searchParams"></param>
        /// <returns></returns>
        // GET: api/Trigger
        public IHttpActionResult Get(int pageNumber, int pageSize, UsageSubscriptionPaymentSearchParams searchParams, string userId = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be equal or grater than 1");

            var page = this._subscriptionVmService.GetPageUsageSubscriptionPayment(pageNumber, pageSize, userId, searchParams);
            var pageModel = AutoMapper.Mapper.Map<PageModel<UsageSubscriptionPaymentView>>(page);

            return this.Ok(pageModel);
        }
    }
}