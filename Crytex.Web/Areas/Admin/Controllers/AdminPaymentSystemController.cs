using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminPaymentSystemController : ApiController
    {
        private readonly IPaymentService _paymentService;

        public AdminPaymentSystemController(IPaymentService paymentService)
        {
            this._paymentService = paymentService;
        }


        /// <summary>
        /// Получить список доступных платежных систем
        /// </summary>
        /// <returns></returns>
        /// GET: api/AdminPaymentSystem
        [HttpGet]
        [ResponseType(typeof(IEnumerable<PaymentSystemView>))]
        public IHttpActionResult PaymentSystems()
        {
            var systems = _paymentService.GetPaymentSystems();
            var model = AutoMapper.Mapper.Map<IEnumerable<PaymentSystemView>>(systems);

            return Ok(model);
        }

        // Put: api/AdminPaymentSystem
        /// <summary>
        /// Изменение состояния платежной системы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult ChangeStatePaymentSystem(string id)
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
    }
}
