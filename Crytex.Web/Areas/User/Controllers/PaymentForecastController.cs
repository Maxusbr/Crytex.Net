using AutoMapper;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System.Web.Http;

namespace Crytex.Web.Areas.User.Controllers
{
    public class PaymentForecastController : UserCrytexController
    {
        private readonly IPaymentForecastService _paymentForecastService;

        public PaymentForecastController(IPaymentForecastService paymentForecastService)
        {
            this._paymentForecastService = paymentForecastService;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var userId = this.CrytexContext.UserInfoProvider.GetUserId();
            var forecast = this._paymentForecastService.GetForecastForUser(userId);
            var viewModel = Mapper.Map<PaymentForecastViewModel>(forecast);

            return this.Ok(viewModel);
        }
    }
}