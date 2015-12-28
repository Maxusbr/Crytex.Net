using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System.Web.Http;
using Crytex.Model.Enums;
using Crytex.Service.Model;

namespace Crytex.Web.Areas.Admin
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
        // GET: api/AdminUsageSubscriptionPaymentController
        public IHttpActionResult Get(int pageNumber, int pageSize, [FromUri]UsageSubscriptionPaymentSearchParams searchParams, string userId = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be equal or grater than 1");

            if (searchParams != null && searchParams.GroupingType == UsageSubscriptionPaymentGroupingTypes.GroupByPeriodAndSubscriptionVm)
            {
                // Group by SubscriptionVm and then by period
                var page = this._subscriptionVmService.GetPageUsageSubscriptionPaymentByVmPeriod(pageNumber, pageSize, userId, searchParams);
                var pageModel = AutoMapper.Mapper.Map<PageModel<UsageSubscriptionPaymentGroupByVmView>>(page);
                return this.Ok(pageModel);
            }
            else if (searchParams != null && searchParams.GroupingType == UsageSubscriptionPaymentGroupingTypes.GroupByPeriod)
            {
                // Group by by period
                var page = this._subscriptionVmService.GetPageUsageSubscriptionPaymentByPeriod(pageNumber, pageSize, userId, searchParams);
                var pageModel = AutoMapper.Mapper.Map<PageModel<UsageSubscriptionPaymentByPeriodView>>(page);
                return this.Ok(pageModel);
            }
            else
            {
                // Don't group
                var page = this._subscriptionVmService.GetPageUsageSubscriptionPayment(pageNumber, pageSize, userId, searchParams);
                var pageModel = AutoMapper.Mapper.Map<PageModel<UsageSubscriptionPaymentView>>(page);
                return this.Ok(pageModel);
            }

            return this.Ok();
        }

        
    }
}