using AutoMapper;
using Crytex.Model.Models.Biling;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Service.Models;
using Crytex.Web.Models;
using Crytex.Web.Models.JsonModels;
using PagedList;
using System;
using System.Web.Http;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminSubscriptionVmController : AdminCrytexController
    {
        private readonly ISubscriptionVmService _subscriptionVmService;

        public AdminSubscriptionVmController(ISubscriptionVmService subscriptionVmService)
        {
            this._subscriptionVmService = subscriptionVmService;
        }

        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                return this.BadRequest("Invalid Guid format");

            var sub = this._subscriptionVmService.GetById(guid);
            var model = AutoMapper.Mapper.Map<SubscriptionVmViewModel>(sub);

            return this.Ok(model);
        }

        [HttpGet]
        public IHttpActionResult Get(int pageNumber, int pageSize, string userId = null, [FromUri]SubscriptionVmSearchParams searchParams = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be grater than 1");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            IPagedList<SubscriptionVm> subscriptions = null;
            if (searchParams != null)
            {
                subscriptions = this._subscriptionVmService.GetPage(pageNumber, pageSize, userId, searchParams);
            }
            else
            {
                subscriptions = this._subscriptionVmService.GetPage(pageNumber, pageSize, userId);
            }

            var viewTransactions = AutoMapper.Mapper.Map<PageModel<SubscriptionVmViewModel>>(subscriptions);
            return Ok(viewTransactions);
        }

        /// <summary>
        /// Покупка подписки админом для пользователя
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Post(SubscriptionBuyOptionsAdminViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            if (model.SubscriptionType == SubscriptionType.Fixed && (model.AutoProlongation == null || model.SubscriptionsMonthCount == null))
            {
                return this.BadRequest();
            }
            var buyOptions = Mapper.Map<SubscriptionBuyOptions>(model);
            buyOptions.BoughtByAdmin = true;
            buyOptions.AdminUserId = this.CrytexContext.UserInfoProvider.GetUserId();
            var newSubscription = this._subscriptionVmService.BuySubscription(buyOptions);

            return this.Ok(new { Id = newSubscription.Id });
        }

        /// <summary>
        /// Продление подписки админом для пользователя
        /// </summary>
        [HttpPut]
        public IHttpActionResult Put(SubscriptionProlongateOptionsViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var prolongateOptions = Mapper.Map<SubscriptionProlongateOptions>(model);
            prolongateOptions.ProlongatedByAdmin = true;
            prolongateOptions.AdminUserId = this.CrytexContext.UserInfoProvider.GetUserId();
            this._subscriptionVmService.ProlongateFixedSubscription(prolongateOptions);

            return this.Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                return this.BadRequest("Invalid Guid format");

            this._subscriptionVmService.DeleteSubscription(guid);

            return Ok();
        }
    }
}