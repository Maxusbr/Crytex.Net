using Project.Service.IService;
using Project.Web.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Project.Web.Controllers.Api
{
    public class CreditPaymentOrderController : ApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ApplicationUserManager _userManager;

        public CreditPaymentOrderController(IPaymentService paymentService, ApplicationUserManager userManager)
        {
            this._paymentService = paymentService;
            this._userManager = userManager;
        }

        [HttpGet]
        public HttpResponseMessage Get(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                this.ModelState.AddModelError("", "PageNumber and PageSize must be grater than 1");
            }
            else
            {
                var page = this._paymentService.GetPage(pageNumber, pageSize);
                var viewModel = AutoMapper.Mapper.Map<PageModel<CreditPaymentOrderViewModel>>(page);
                return Request.CreateResponse(HttpStatusCode.OK, viewModel);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        // GET: api/CreditPaymentOrder/5
        [HttpGet]
        public HttpResponseMessage Get(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                this.ModelState.AddModelError("id", "Invalid Guid format");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var order = this._paymentService.GetCreditPaymentOrderById(guid);
            var model = AutoMapper.Mapper.Map<CreditPaymentOrderViewModel>(order);

            return this.Request.CreateResponse(HttpStatusCode.OK, model);
        }

        // POST: api/CreditPaymentOrder
        [HttpPost]
        public HttpResponseMessage Create(CreditPaymentOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userName = this.User.Identity.Name;
                var user = this._userManager.Users.Single(u => u.UserName == userName);
                var newOrder = this._paymentService.CreateCreditPaymentOrder(model.CashAmount.Value, user.Id, model.PaymentSystem.Value);

                return Request.CreateResponse(HttpStatusCode.Created, new { id = newOrder.Guid });
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        // DELETE: api/CreditPaymentOrder/5
        [HttpDelete]
        public HttpResponseMessage Delete(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                this.ModelState.AddModelError("id", "Invalid Guid format");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            this._paymentService.DeleteCreditPaymentOrderById(guid);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
