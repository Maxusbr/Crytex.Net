using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using Crytex.Model.Models.Biling;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// Операции с системами платажей
    /// </summary>
    public class AdminPaymentSystemController : AdminCrytexController
    {
        private readonly IPaymentSystemService _paymentSystemService;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="paymentSystemService"></param>
        public AdminPaymentSystemController(IPaymentSystemService paymentSystemService)
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
        public IHttpActionResult PaymentSystems()
        {
            var systems = _paymentSystemService.GetPaymentSystems();
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

        /// <summary>
        /// Создание платёжной системы
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Post(PaymentSystemView model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymentSystem = Mapper.Map<PaymentSystem>(model);
            paymentSystem = _paymentSystemService.Create(paymentSystem);

            return Ok(new { id = paymentSystem.Id });
        }

        /// <summary>
        /// Обновление платёжной системы
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdatePaymentSystem(PaymentSystemView model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var paymentSystem = Mapper.Map<PaymentSystem>(model);
            _paymentSystemService.Update(paymentSystem);

            return Ok();
        }

        /// <summary>
        /// Удаление платёжной системы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeletePaymentSystem(string id)
        {
            _paymentSystemService.Delete(id);

            return Ok();
        }




    }
}
