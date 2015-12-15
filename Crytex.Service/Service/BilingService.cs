using System;
using System.Linq.Expressions;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Model.Models.Biling;
using Crytex.Service.Extension;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Web.Models.JsonModels;
using PagedList;

namespace Crytex.Service.Service
{
    public class BilingService : IBilingService
    {
        private IBillingTransactionRepository _billingTransactionRepo;
        private IApplicationUserRepository _applicationUserRepository;
        private IUnitOfWork _unitOfWork;

        public BilingService(IUnitOfWork unitOfWork, IBillingTransactionRepository billingTransactionRepo, IApplicationUserRepository applicationUserRepository)
        {
            _billingTransactionRepo = billingTransactionRepo;
            _applicationUserRepository = applicationUserRepository;
            _unitOfWork = unitOfWork;
        }

        public BillingTransaction GetTransactionById(Guid id)
        {
            var transaction = this._billingTransactionRepo.GetById(id);
            if (transaction == null)
            {
                throw new InvalidIdentifierException(string.Format("Transaction with id={0} doesnt exist.", id));
            }
            return transaction;
        }

        public IPagedList<BillingTransaction> GetPageBillingTransaction(int pageNumber, int pageSize, BillingSearchParams searchParams = null)
        {
            var page = new PageInfo(pageNumber, pageSize);

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

            var transactionList = this._billingTransactionRepo.GetPage(page, where, x => x.Id);
            foreach (var transaction in transactionList)
            {
                transaction.User = _applicationUserRepository.GetById(transaction.UserId);
            }       
            return transactionList;
        }

        public void UpdateUserBalance(UpdateUserBalance data) //не забыть ! обновить баланс user как только добавим это поле
        {
            var user = this._applicationUserRepository.Get(u => u.Id == data.UserId);

            if (user == null)
            {
                throw new InvalidIdentifierException(string.Format("User width Id={0} doesn't exists", data.UserId));
            }
            
            var transaction = new BillingTransaction
            {
                UserId = data.UserId,
                CashAmount = Math.Abs(data.Amount),
                TransactionType = (data.Amount > 0)? BillingTransactionType.ReplenishmentFromAdmin:BillingTransactionType.WithdrawByAdmin,
                Description = "Admin Transaction",
                Date = DateTime.UtcNow
            };

            _billingTransactionRepo.Add(transaction);
            
            _unitOfWork.Commit();
        }


    }
}
