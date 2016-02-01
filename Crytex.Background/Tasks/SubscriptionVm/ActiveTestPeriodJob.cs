using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models.Biling;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Quartz;

namespace Crytex.Background.Tasks.SubscriptionVm
{
    [DisallowConcurrentExecution]
    public class ActiveTestPeriodJob : IJob
    {
        private readonly IBilingService _bilingService;

        public ActiveTestPeriodJob(IBilingService bilingService)
        {
            _bilingService = bilingService;
        }

        public void Execute(IJobExecutionContext context)
        {
            var option = new BillingSearchParams { BillingTransactionType = (int)BillingTransactionType.TestPeriod, SubscriptionVmMonthCount = 1 };
            var tests = _bilingService.SearchBillingTransactions(option).Where(el =>
                el.Date.AddDays(el.SubscriptionVmMonthCount ?? Int32.MaxValue) >= DateTime.UtcNow);
            foreach (var transaction in tests)
            {
                var totalbalance = transaction.UserBalance;
                option.DateFrom = transaction.Date;
                option.UserId = transaction.UserId;
                option.DateTo = DateTime.UtcNow;
                option.BillingTransactionType = (int) BillingTransactionType.OneTimeDebiting;
                var userTransactions = _bilingService.SearchBillingTransactions(option);
                if (userTransactions.Any())
                    totalbalance += userTransactions.Sum(o => o.CashAmount);
                transaction.SubscriptionVmMonthCount = 0;
                _bilingService.CancelTestPeriod(new UpdateUserBalance
                {
                    UserId = transaction.UserId,
                    Amount = totalbalance
                }, transaction);
            }
        }
    }
}
