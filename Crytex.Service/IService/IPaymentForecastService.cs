using Crytex.Service.Model;

namespace Crytex.Service.IService
{
    public interface IPaymentForecastService
    {
        PaymentForecast GetForecastForUser(string userId);
    }
}
