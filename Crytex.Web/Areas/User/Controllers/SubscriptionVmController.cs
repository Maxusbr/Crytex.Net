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
using Crytex.Model.Exceptions;
using Crytex.Web.Helpers;
using Crytex.Model.Models;

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
            try
            {
                var newSubscription = this._subscriptionVmService.BuySubscription(buyOptions);


                return this.Ok(new { Id = newSubscription.Id });
            }
            catch (TransactionFailedException ex)
            {
                return new CrytexResult(ServerTypesResult.NotEnoughMoney);
            }
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

        /// <summary>
        /// Обновление статуса машины
        /// </summary>
        [HttpPost]
        public IHttpActionResult UpdateMachineStatus(UpdateMachineStatusOptions model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            Guid guid;
            if (!Guid.TryParse(model.SubscriptionId, out guid))
                return this.BadRequest("Invalid Guid format");
            switch (model.Status)
            {
                case TypeChangeStatus.Start:
                    this._subscriptionVmService.StartSubscriptionMachine(guid);
                    break;
                case TypeChangeStatus.PowerOff:
                    this._subscriptionVmService.PowerOffSubscriptionMachine(guid);
                    break;
                case TypeChangeStatus.Reload:
                    this._subscriptionVmService.ResetSubscriptionMachine(guid);
                    break;
                case TypeChangeStatus.Stop:
                    this._subscriptionVmService.StopSubscriptionMachine(guid);
                    break;
                default:
                    return BadRequest("Invalid status");
            }

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult UpdateSubscriptionConfiguration(MachineConfigUpdateViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var serviceOptions = Mapper.Map<UpdateMachineConfigOptions>(model);
            this._subscriptionVmService.UpdateSubscriptionConfig(model.SubscriptionId.Value, serviceOptions);

            return Ok();
        }

        /// <summary>
        /// Включение тестового периода
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddTestPeriod(TestPeriodViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var serviceOptions = Mapper.Map<TestPeriodOptions>(model);
            this._subscriptionVmService.AddTestPeriod(serviceOptions);

            return Ok();
        }

        /// <summary>
        /// Обновление подписки пользователем
        /// </summary>
        [HttpPost]
        public IHttpActionResult UpdateSubscription(SubscriptionUpdateOptions model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            this._subscriptionVmService.UpdateSubscriptionData(model);

            return this.Ok();
        }

        [HttpPost]
        public IHttpActionResult UpdateSubscriptionBackupStoragePeriod(UpdateSubscriptionBackupStoragePeriodModel model)
        {
            if (model.NewPeriodDays < 0)
            {
                return BadRequest("Backup storage period cannot be negative");
            }

            _subscriptionVmService.UpdateSubscriptionBackupStoragePeriod(model.SubscriptionId, model.NewPeriodDays);

            return this.Ok();
        }
    }


}