using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models.Biling;
using Crytex.Service.Extension;
using Crytex.Service.IService;
using Crytex.Service.Model;
using PagedList;

namespace Crytex.Service.Service
{
    public class BilingService : IBilingService
    {
        private IBillingTransactionRepository _billingTransactionRepo;
        private IUnitOfWork _unitOfWork;

        public BilingService(IUnitOfWork unitOfWork, IBillingTransactionRepository billingTransactionRepo)
        {
            _billingTransactionRepo = billingTransactionRepo;
            _unitOfWork = unitOfWork;
        }

        public IPagedList<BillingTransaction> GetPageBillingTransaction(int pageNumber, int pageSize, BillingSearchParams searchParams = null)
        {
            var page = new Page(pageNumber, pageSize);

            Expression<Func<BillingTransaction, bool>> where = x => true;

            if (searchParams != null)
            {
                if (searchParams.UserId != null)
                {                   
                    where = where.And(x => x.UserId == searchParams.UserId);
                }

                if (searchParams.BillingTransactionType != null)
                {
                    where = where.And(x => (int)x.TransactionType == searchParams.BillingTransactionType);
                }

                if (searchParams.DateFrom != null)
                {
                    where = where.And(x => x.Date >= searchParams.DateFrom);
                }

                if (searchParams.DateTo != null)
                {
                    where = where.And(x => x.Date <= searchParams.DateTo);
                }                               
            }

            var list = this._billingTransactionRepo.GetPage(page, where, x => x.Id);
            return list;
        }


    }
}
