using AutoMapper;
using Crytex.Model.Models.Biling;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Service.Models;
using Crytex.Web.Models.JsonModels;
using Microsoft.Practices.Unity;
using PagedList;
using System;
using System.Web.Http;

namespace Crytex.Web.Areas.User.Controllers
{
    public class SubscriptionVmController : UserCrytexController
    {
        private readonly ISubscriptionVmService _subscriptionVmService;

        public SubscriptionVmController([Dependency("Secured")]ISubscriptionVmService subscriptionVmService)
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
        public IHttpActionResult Get(int pageNumber, int pageSize, [FromUri]SubscriptionVmSearchParams searchParams = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be grater than 1");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = this.CrytexContext.UserInfoProvider.GetUserId();
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

        [HttpPost]
        public IHttpActionResult Post(SubscriptionBuyOptionsUserViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var buyOptions = Mapper.Map<SubscriptionBuyOptions>(model);
            buyOptions.BoughtByAdmin = false;
            buyOptions.UserId = this.CrytexContext.UserInfoProvider.GetUserId();
            var newSubscription = this._subscriptionVmService.BuySubscription(buyOptions);

            return this.Ok(new { Id = newSubscription.Id });
        }

        /// <summary>
        /// Продление подписки
        /// </summary>
        [HttpPut]
        public IHttpActionResult Put(SubscriptionProlongateOptionsViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var prolongateOptions = Mapper.Map<SubscriptionProlongateOptions>(model);
            prolongateOptions.ProlongatedByAdmin = false;
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