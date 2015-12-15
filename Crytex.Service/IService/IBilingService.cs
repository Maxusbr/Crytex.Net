using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        BillingTransaction AddUserTransaction(BillingTransactionType type, decimal cashAmount, string description, string userId, string adminUserId = null);
    }
}
