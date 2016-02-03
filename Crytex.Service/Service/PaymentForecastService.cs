using System;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Model.Models.Biling;

namespace Crytex.Service.Service
{
    class PaymentForecastService : IPaymentForecastService
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly IGameServerService _gameServerService;
        private readonly ISubscriptionVmService _subscriptionVmService;
        private readonly IWebHostingService _webHostingService;

        public PaymentForecastService(IApplicationUserService appUserService, ISubscriptionVmService subscriptionVmService,
            IGameServerService gameServerService, IWebHostingService webHostingService)
        {
            this._applicationUserService = appUserService;
            this._subscriptionVmService = subscriptionVmService;
            this._gameServerService = gameServerService;
            this._webHostingService = webHostingService;
        }

        public PaymentForecast GetForecastForUser(string userId)
        {
            var forecast = new PaymentForecast();
            var user = this._applicationUserService.GetUserById(userId);
            forecast.CurrentBalance = user.Balance;

            var usageSubs = this._subscriptionVmService.GetAllSubscriptionsByTypeAndUserId(SubscriptionType.Usage, user.Id);
            foreach(var usageSub in usageSubs)
            {
                // Usage sub day forecast
                var usageSubPaymentOneDayForecast = new SubscriptionPaymentForecast();
                var usageSubHourPrice = this._subscriptionVmService.GetUsageSubscriptionHourPriceTotal(usageSub);
                usageSubPaymentOneDayForecast.PaymentForecast =  usageSubHourPrice * 24;
                usageSubPaymentOneDayForecast.SubscriptionVm = usageSub;

                forecast.UsageSubscriptionOneDayForecasts.Add(usageSubPaymentOneDayForecast);

                // Usage sub month forecast
                var usageSubPaymentMonthForecast = new SubscriptionPaymentForecast();
                usageSubPaymentMonthForecast.PaymentForecast = usageSubHourPrice * 24 * 30;
                usageSubPaymentMonthForecast.SubscriptionVm = usageSub;

                forecast.UsageSubscriptionsMonthForecasts.Add(usageSubPaymentMonthForecast);
            }

            var fixedSubs = this._subscriptionVmService.GetAllSubscriptionsByTypeAndUserId(SubscriptionType.Fixed, user.Id);
            foreach(var fixedSub in fixedSubs)
            {
                var fixedSubPaymentForecast = new SubscriptionPaymentForecast();
                var fixedSubMonthPrice = this._subscriptionVmService.GetFixedSubscriptionMonthPriceTotal(fixedSub);
                fixedSubPaymentForecast.PaymentForecast = fixedSubMonthPrice;
                fixedSubPaymentForecast.SubscriptionVm = fixedSub;

                forecast.FixedSubscriptionsMonthForecasts.Add(fixedSubPaymentForecast);
            }

            var gameServers = this._gameServerService.GetAllByUserId(user.Id);
            foreach(var gameServer in gameServers)
            {
                var gameServerPaymentForecast = new GameServerPaymentForecast();
                var gameServerMonthPrice = this._gameServerService.GetGameServerMonthPrice(gameServer);
                gameServerPaymentForecast.PaymentForecast = gameServerMonthPrice;
                gameServerPaymentForecast.GameServer = gameServer;

                forecast.GameServerPaymentForecasts.Add(gameServerPaymentForecast);
            }

            var webHostings = this._webHostingService.GetAllByUserId(user.Id);
            foreach(var webHosting in webHostings)
            {
                var webHostingPaymentForecast = new WebHostingPaymentForecast();
                var webHostingMonthPrice = this._webHostingService.GetWebHostingMonthPrice(webHosting);
                webHostingPaymentForecast.PaymentForecast = webHostingMonthPrice;
                webHostingPaymentForecast.WebHosting = webHosting;

                forecast.WebHostingPaymentForecasts.Add(webHostingPaymentForecast);
            }

            return forecast;
        }
    }
}
