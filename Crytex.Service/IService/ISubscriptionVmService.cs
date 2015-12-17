using Crytex.Model.Models.Biling;
using Crytex.Service.Model;

namespace Crytex.Service.IService
{
    public interface ISubscriptionVmService
    {
        SubscriptionVm BuySubscription(SubscriptionBuyOptions options);
    }
}
