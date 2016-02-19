using System.Linq;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Service.Model;
using Microsoft.Practices.Unity;
using AutoMapper;

namespace Crytex.Web.Areas.User
{
    public class PaymentController : UserCrytexController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController([Dependency("Secured")]IPaymentService paymentService)
        {
            this._paymentService = paymentService;
        }

        /// <summary>
        /// Получение списка Payment
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [ResponseType(typeof(PageModel<PaymentView>))]
        // GET: api/Payment
        public IHttpActionResult Get(int pageNumber, int pageSize, SearchPaymentParams filter)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("PageNumber and PageSize must be grater than 1");
            }

            filter.UserId = CrytexContext.UserInfoProvider.GetUserId();

            var page = this._paymentService.GetPage(pageNumber, pageSize, filter);
            var viewModel = AutoMapper.Mapper.Map<PageModel<PaymentView>>(page);

            return Ok(viewModel);
        }

        // GET: api/Payment/5
        public IHttpActionResult Get(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                this.ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }

            var order = this._paymentService.GetCreditPaymentOrderById(guid);

            var model = AutoMapper.Mapper.Map<PaymentView>(order);

            return Ok(model);
        }

        // POST: api/Payment
        public IHttpActionResult Post([FromBody]PaymentView model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Guid paymentGuid;
            if (!Guid.TryParse(model.PaymentSystemId, out paymentGuid))
            {
                this.ModelState.AddModelError("PaymentSystemId", "Invalid Guid format");
            }
            var userId = this.CrytexContext.UserInfoProvider.GetUserId();
            var newOrder = this._paymentService.CreateCreditPaymentOrder(model.CashAmount.Value, userId, paymentGuid);

            return Ok(new { id = newOrder.Guid });
        }

        // DELETE: api/Payment/5
        public IHttpActionResult Delete(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                this.ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }
            this._paymentService.DeleteCreditPaymentOrderById(guid);

            return Ok();
        }

        // GET: api/Payment/PaymentSystems
        /// <summary>
        /// Получить список доступных платежных систем
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult PaymentSystems()
        {
            var systems = _paymentService.GetPaymentSystems(true);
            var model = AutoMapper.Mapper.Map<IEnumerable<PaymentSystemView>>(systems);

            return Ok(model);
        }

        [HttpGet]
        [ResponseType(typeof(PageModel<BillingTransactionInfoViewModel>))]
        [Route("api/User/Payment/method/billingTransactionInfo")]
        public IHttpActionResult BillingTransactionInfos(int pageNumber, int pageSize, DateTime? from = null, DateTime? to = null)
        {
            var userId = this.CrytexContext.UserInfoProvider.GetUserId();
            var infos = this._paymentService.GetUserBillingTransactionInfosPage(userId, pageNumber, pageSize, from, to);
            var models = Mapper.Map<PageModel<BillingTransactionInfoViewModel>>(infos);

            return Ok(models);
        }
    }
}
