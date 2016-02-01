using System;
using System.Collections.Generic;
using Crytex.Model.Models.Biling;
using Crytex.Service.Model;
using PagedList;

namespace Crytex.Service.IService
{
    public interface IBilingService
    {
        IPagedList<BillingTransaction> GetPageBillingTransaction(int pageNumber, int pageSize, BillingSearchParams searchParams = null);
        BillingTransaction GetTransactionById(Guid id);
        BillingTransaction UpdateUserBalance(UpdateUserBalance data);
        BillingTransaction AddUserTransaction(BillingTransaction transaction);
        void RevertUserTransaction(Guid transactionId);
        void UpdateTransactionSubscriptionId(Guid transactionId, Guid subscriptionId);
        IEnumerable<BillingTransaction> SearchBillingTransactions(BillingSearchParams searchParams);
        BillingTransaction AddTestPeriod(TestPeriodOptions options);
        void CancelTestPeriod(UpdateUserBalance data, BillingTransaction transaction);
    }
}
