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
        StaticPagedList<UsageSubscriptionPaymentContainer> GetPageUsageSubscriptionPaymentByPeriod(int pageNumber, int pageSize, string userId = null, UsageSubscriptionPaymentSearchParams searchParams = null);
        IEnumerable<SubscriptionVm> GetAllByStatus(SubscriptionVmStatus status);
        StaticPagedList<UsageSubscriptionPaymentGroupByVmContainer> GetPageUsageSubscriptionPaymentByVmPeriod(int pageNumber, int pageSize, string userId = null, UsageSubscriptionPaymentSearchParams searchParams = null);
        IPagedList<SubscriptionVm> GetPage(int pageNumber, int pageSize, string userId = null, SubscriptionVmSearchParams searchParams = null);
        IEnumerable<SubscriptionVm> GetSubscriptionsByStatusAndType(SubscriptionVmStatus status, SubscriptionType type);
        void UpdateSubscriptionStatus(Guid subId, SubscriptionVmStatus status, DateTime? endDate = null);
        IEnumerable<SubscriptionVm> GetAllSubscriptionsByTypeAndUserId(SubscriptionType subscriptionType, string userId = null);
        void UpdateSubscriptionData(SubscriptionUpdateOptions model);
        void UpdateUsageSubscriptionBalance(Guid subId);
        void ProlongateFixedSubscription(SubscriptionProlongateOptions options);
        void AutoProlongateFixedSubscription(Guid subId);
        void PrepareSubscriptionForDeletion(Guid subId);
        void DeleteSubscription(Guid subId);
        void StartSubscriptionMachine(Guid subsciptionId);
        void StopSubscriptionMachine(Guid subsciptionId);
        void PowerOffSubscriptionMachine(Guid subsciptionId);
        void ResetSubscriptionMachine(Guid subsciptionId);
        void UpdateSubscriptionMachineConfig(Guid subscriptionId, UpdateMachineConfigOptions options);
        void AddTestPeriod(TestPeriodOptions options);
        decimal GetUsageSubscriptionHourPriceTotal(SubscriptionVm sub);
        decimal GetFixedSubscriptionMonthPriceTotal(SubscriptionVm sub);
    }
}
