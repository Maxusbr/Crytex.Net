using System;
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
    }
}
