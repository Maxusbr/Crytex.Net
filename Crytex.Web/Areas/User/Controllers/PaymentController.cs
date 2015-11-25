using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Service.Model;


namespace Crytex.Web.Areas.User
{
    public class PaymentController : UserCrytexController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
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

            if (order.UserId != CrytexContext.UserInfoProvider.GetUserId())
            {
                return BadRequest("Are not allowed for this action");
            }

            var model = AutoMapper.Mapper.Map<PaymentView>(order);

            return Ok(model);
        }

        // POST: api/Payment
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
    }
}
