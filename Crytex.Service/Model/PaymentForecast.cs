using Crytex.Model.Models.Biling;
using Crytex.Model.Models.WebHostingModels;
using System.Collections.Generic;
using Crytex.Model.Models.GameServers;

namespace Crytex.Service.Model
{
    public class PaymentForecast
    {
        public decimal CurrentBalance { get; set; }
        /// <summary>
        /// Прогноз расходов по имеющимся usage-подпискам на день
        /// </summary>
        public List<SubscriptionPaymentForecast> UsageSubscriptionOneDayForecasts { get; set; } = new List<SubscriptionPaymentForecast>();
        /// <summary>
        /// Прогноз расходов по имеющимся usage-подпискам на месяц
        /// </summary>
        public List<SubscriptionPaymentForecast> UsageSubscriptionsMonthForecasts { get; set; } = new List<SubscriptionPaymentForecast>();
        /// <summary>
        /// Прогноз расходов по имеющимся fixed-подпискам на месяц
        /// </summary>
        public List<SubscriptionPaymentForecast> FixedSubscriptionsMonthForecasts { get; set; } = new List<SubscriptionPaymentForecast>();
        /// <summary>
        /// Прогноз расходов по имеющимся игровым серверам на месяц
        /// </summary>
        public List<GameServerPaymentForecast> GameServerPaymentForecasts { get; set; } = new List<GameServerPaymentForecast>();
        /// <summary>
        /// Прогноз расходов по имеющимся веб-хостингам на месяц
        /// </summary>
        public List<WebHostingPaymentForecast> WebHostingPaymentForecasts { get; set; } = new List<WebHostingPaymentForecast>();
    }

    public class PaymentForecastBase
    {
        public decimal PaymentForecast { get; set; }
    }

    public class SubscriptionPaymentForecast : PaymentForecastBase
    {
        public SubscriptionVm SubscriptionVm { get; set; }
    }

    public class GameServerPaymentForecast : PaymentForecastBase
    {
        public GameServer GameServer { get; set; }
    }

    public class WebHostingPaymentForecast : PaymentForecastBase
    {
        public WebHosting WebHosting { get; set; }
    }
}
