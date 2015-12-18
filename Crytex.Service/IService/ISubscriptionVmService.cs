using System;
using Crytex.Model.Models.Biling;
using Crytex.Service.Model;
using PagedList;
using Crytex.Service.Models;

namespace Crytex.Service.IService
{
    public interface ISubscriptionVmService
    {
        SubscriptionVm BuySubscription(SubscriptionBuyOptions options);
        SubscriptionVm GetById(Guid guid);
        IPagedList<SubscriptionVm> GetPage(int pageNumber, int pageSize, string userId = null, SubscriptionVmSearchParams searchParams = null);
    }
}
