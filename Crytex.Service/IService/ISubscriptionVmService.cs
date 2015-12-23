using System;
using Crytex.Model.Models.Biling;
using Crytex.Service.Model;
using PagedList;
using Crytex.Service.Models;
using System.Collections.Generic;

namespace Crytex.Service.IService
{
    public interface ISubscriptionVmService
    {
        SubscriptionVm BuySubscription(SubscriptionBuyOptions options);
        SubscriptionVm GetById(Guid guid);
        IPagedList<UsageSubscriptionPayment> GetPageUsageSubscriptionPayment(int pageNumber, int pageSize, string userId = null, UsageSubscriptionPaymentSearchParams searchParams = null);
        IPagedList<SubscriptionVm> GetPage(int pageNumber, int pageSize, string userId = null, SubscriptionVmSearchParams searchParams = null);
        IEnumerable<SubscriptionVm> GetSubscriptionsByStatusAndType(SubscriptionVmStatus status, SubscriptionType type);
        void UpdateSubscriptionStatus(Guid subId, SubscriptionVmStatus status, DateTime? endDate = null);
        IEnumerable<SubscriptionVm> GetAllFixedSubscriptions();
        void UpdateUsageSubscriptionBalance(Guid subId);
        void AutoProlongateSubscription(Guid subId);
        void PrepareSubscriptionForDeletion(Guid subId);
        void DeleteSubscription(Guid subId);
    }
}
