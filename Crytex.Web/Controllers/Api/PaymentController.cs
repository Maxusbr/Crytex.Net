using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Crytex.Web.Controllers.Api
{
    public class PaymentController : CrytexApiController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            this._paymentService = paymentService;
        }

        // GET: api/CreditPaymentOrder
        public IHttpActionResult Get(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("PageNumber and PageSize must be grater than 1");
            }

            var page = this._paymentService.GetPage(pageNumber, pageSize);
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
            var userId = this.CrytexContext.UserInfoProvider.GetUserId();
            var newOrder = this._paymentService.CreateCreditPaymentOrder(model.CashAmount.Value, userId, model.PaymentSystem.Value);

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
    }
}
