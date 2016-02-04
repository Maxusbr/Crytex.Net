using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Service.Model;

namespace Crytex.Web.Areas.Admin
{
    public class AdminPaymentController : AdminCrytexController
    {
        private readonly IPaymentService _paymentService;

        public AdminPaymentController(IPaymentService paymentService)
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
        // GET: api/CreditPaymentOrder
        public IHttpActionResult Get(int pageNumber, int pageSize, [FromUri]SearchPaymentParams filter)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("PageNumber and PageSize must be grater than 1");
            }

            var page = this._paymentService.GetPage(pageNumber, pageSize, filter);
            var viewModel = AutoMapper.Mapper.Map<PageModel<PaymentView>>(page);

            return Ok(viewModel);
        }

        // GET: api/CreditPaymentOrder/5
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

        // POST: api/CreditPaymentOrder
        public IHttpActionResult Post([FromBody]PaymentView model)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            Guid guid;
            if (!Guid.TryParse(model.PaymentSystemId, out guid))
            {
                this.ModelState.AddModelError("PaymentSystemId", "Invalid Guid format");
            }
            var userId = this.CrytexContext.UserInfoProvider.GetUserId();
            var newOrder = this._paymentService.CreateCreditPaymentOrder(model.CashAmount.Value, userId, guid);

            return Ok(new { id = newOrder.Guid });
        }

        // DELETE: api/CreditPaymentOrder/5
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

        // GET: api/AdminPayment/PaymentSystems
        /// <summary>
        /// Получить список доступных платежных систем
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult PaymentSystems()
        {
            var systems = _paymentService.GetPaymentSystems();
            var model = AutoMapper.Mapper.Map<IEnumerable<PaymentSystemView>>(systems);

            return Ok(model);
        }

        // POST: api/AdminPayment/EnablePaymentSystem
        /// <summary>
        /// Включить платежную систему 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult EnablePaymentSystem(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                ModelState.AddModelError("Id", "Invalid Guid format");
                return BadRequest(ModelState);
            }
            _paymentService.EnableDisablePaymentSystem(guid, true);

            return Ok();
        }

        // POST: api/AdminPayment/EnablePaymentSystem
        /// <summary>
        /// Выключить платежную систему 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult DisablePaymentSystem(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                ModelState.AddModelError("Id", "Invalid Guid format");
                return BadRequest(ModelState);
            }
            _paymentService.EnableDisablePaymentSystem(guid, false);

            return Ok();
        }

    }
}
