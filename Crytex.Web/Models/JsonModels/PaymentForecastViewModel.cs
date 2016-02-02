using System;
using System.Collections.Generic;

namespace Crytex.Web.Models.JsonModels
{
    public class PaymentForecastViewModel
    {
        public decimal CurrentBalance { get; set; }
        /// <summary>
        /// Прогноз расходов по имеющимся usage-подпискам на день
        /// </summary>
        public IEnumerable<SubscriptionPaymentForecastViewModel> UsageSubscriptionOneDayForecasts { get; set; }
        /// <summary>
        /// Прогноз расходов по имеющимся usage-подпискам на месяц
        /// </summary>
        public List<SubscriptionPaymentForecastViewModel> UsageSubscriptionsMonthForecasts { get; set; }
        /// <summary>
        /// Прогноз расходов по имеющимся fixed-подпискам на месяц
        /// </summary>
        public List<SubscriptionPaymentForecastViewModel> FixedSubscriptionsMonthForecasts { get; set; }
        /// <summary>
        /// Прогноз расходов по имеющимся игровым серверам на месяц
        /// </summary>
        public List<GameServerPaymentForecastViewModel> GameServerPaymentForecasts { get; set; }
        /// <summary>
        /// Прогноз расходов по имеющимся веб-хостингам на месяц
        /// </summary>
        public List<WebHostingPaymentForecastViewModel> WebHostingPaymentForecasts { get; set; } = new List<WebHostingPaymentForecastViewModel>();
    }

    public class PaymentForecastViewModelBase
    {
        public decimal PaymentForecast { get; set; }
    }

    public class SubscriptionPaymentForecastViewModel : PaymentForecastViewModelBase
    {
        public Guid SubscriptionVmId { get; set; }
    }

    public class GameServerPaymentForecastViewModel : PaymentForecastViewModelBase
    {
        public Guid GameServerId { get; set; }
    }

    public class WebHostingPaymentForecastViewModel : PaymentForecastViewModelBase
    {
        public Guid WebHostingId { get; set; }
    }
}