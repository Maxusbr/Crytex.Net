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
using PagedList;
using System.Data.Entity.Infrastructure;
using System.Linq;

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

                if (searchParams.SubscriptionVmId != null && searchParams.SubscriptionVmId != Guid.Empty)
                {
                    where = where.And(x => x.SubscriptionVmId == searchParams.SubscriptionVmId);
                }

                if (searchParams.SubscriptionGameServerId != null && searchParams.SubscriptionGameServerId != Guid.Empty)
                {
                    where = where.And(x => x.GameServerId == searchParams.SubscriptionGameServerId);
                }
            }

            var transactionList = this._billingTransactionRepo.GetPage(page, where, x => x.Id);
            foreach (var transaction in transactionList)
            {
                transaction.User = _applicationUserRepository.GetById(transaction.UserId);
            }
            return transactionList;
        }

        public BillingTransaction AddUserTransaction(BillingTransaction transaction)
        {
            var newTransaction = this.AddUserTransactionInner(transaction.TransactionType, transaction.CashAmount, transaction.Description,
                transaction.UserId, transaction.AdminUserId, transaction.SubscriptionVmId, transaction.SubscriptionVmMonthCount);

            return newTransaction;
        }

        public BillingTransaction UpdateUserBalance(UpdateUserBalance data)
        {
            var transactionType = (data.Amount > 0) ? BillingTransactionType.ReplenishmentFromAdmin : BillingTransactionType.WithdrawByAdmin;
            var cashAmount = Math.Abs(data.Amount);
            var description = "Admin Transaction";

            var transaction = this.AddUserTransactionInner(transactionType, cashAmount, description, data.UserId);

            return transaction;
        }

        private BillingTransaction AddUserTransactionInner(BillingTransactionType type, decimal cashAmount, string description,
            string userId, string adminUserId = null, Guid? subscriptionId = null, int? subscriptionMonthCount = null)
        {
            var user = this.GetUserById(userId);

            var transaction = new BillingTransaction
            {
                CashAmount = cashAmount,
                TransactionType = type,
                Description = description,
                UserId = userId,
                Date = DateTime.UtcNow,
                AdminUserId = adminUserId,
                SubscriptionVmId = subscriptionId,
                SubscriptionVmMonthCount = subscriptionMonthCount
            };
            user.BillingTransactions.Add(transaction);

            bool saveFailed = false;
            var tryCount = 3;
            for(int i = 0; i < tryCount;  i++)
            {
                try
                {
                    saveFailed = false;
                    var userCash = user.Balance + cashAmount;

                    if(userCash < 0)
                    {
                        throw new TransactionFailedException("Not enough money. Transaction failed.");
                    }

                    user.Balance = userCash;
                    this._applicationUserRepository.Update(user);
                    this._unitOfWork.Commit();

                    break;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;
                    ex.Entries.Single().Reload();
                }
            }

            if (saveFailed)
            {
                throw new DbUpdateApplicationException($"Cannot update user balance because of concurrent update requests. Transaction creating is aborted");
            }

            return transaction;
        }

        public void UpdateTransactionSubscriptionId(Guid transactionId, Guid subscriptionId)
        {
            var transaction = this.GetTransactionById(transactionId);
            transaction.SubscriptionVmId = subscriptionId;
            this._billingTransactionRepo.Update(transaction);
            this._unitOfWork.Commit();
        }

        protected ApplicationUser GetUserById(string id)
        {
            var user = this._applicationUserRepository.Get(u => u.Id == id, u => u.BillingTransactions);

            if (user == null)
            {
                throw new InvalidIdentifierException(string.Format("User width Id={0} doesn't exists", id));
            }

            return user;
        }
    }
}
