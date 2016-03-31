using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.User.Controllers
{
    public class UserPaymentSystemController : UserCrytexController
    {
        private readonly IPaymentSystemService _paymentSystemService;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="paymentSystemService"></param>
        public UserPaymentSystemController(IPaymentSystemService paymentSystemService)
        {
            _paymentSystemService = paymentSystemService;
        }

        /// <summary>
        /// Получить список доступных платежных систем
        /// </summary>
        /// <returns></returns>
        /// GET: api/AdminPaymentSystem
        [HttpGet]
        [ResponseType(typeof(IEnumerable<PaymentSystemView>))]
        public IHttpActionResult PaymentSystems(Boolean searchEnabled = false)
        {
            var systems = _paymentSystemService.GetPaymentSystems(searchEnabled);
            var model = AutoMapper.Mapper.Map<IEnumerable<PaymentSystemView>>(systems);

            return Ok(model);
        }

        /// <summary>
        /// Получить платежную систему
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// GET: api/AdminPaymentSystem
        [HttpGet]
        [ResponseType(typeof(PaymentSystemView))]
        public IHttpActionResult PaymentSystem(string id)
        {
            var paymentSystem = _paymentSystemService.GetPaymentSystemById(id);

            return Ok(paymentSystem);
        }
    }
}
