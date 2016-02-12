using PagedList;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Crytex.Model.Models.Biling;
using Crytex.Service.Extension;
using Crytex.Service.Model;
using System.Linq;

namespace Crytex.Service.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IBillingTransactionRepository _billingTransactionRepository;
        private readonly IBilingService _bilingService;
        private readonly IDiscountService _discountService;
        private readonly ICreditPaymentOrderRepository _creditPaymentOrderRepository;
        private readonly IPaymentSystemRepository _paymentSystemRepository;
        private readonly IWebHostingPaymentRepository _webHostingPaymentRepo;
        private readonly IFixedSubscriptionPaymentRepository _fixedSubscriptionPaymentRepository;
        private readonly IUsageSubscriptionPaymentRepository _usageSubscriptionPaymentRepository;
        private readonly ISubscriptionBackupPaymentRepository _subscriptionBackupPaymentRepository;
        private readonly IBoughtPhysicalServerRepository _physicalServerPaymentRepository;
        private readonly IPaymentGameServerRepository _gameServerPaymentRepository;

        public PaymentService(IUnitOfWork unitOfWork, IBillingTransactionRepository billingRepo,
            ICreditPaymentOrderRepository creditPaymentOrderRepo, IPaymentSystemRepository paymentSystemRepository, 
            IDiscountService discountService, IBilingService bilingService, IWebHostingPaymentRepository webHostingPaymentRepo,
            IFixedSubscriptionPaymentRepository fixedSubscriptionPaymentRepository, IUsageSubscriptionPaymentRepository usageSubscriptionPaymentRepository,
            ISubscriptionBackupPaymentRepository subscriptionBackupPaymentRepository, IBoughtPhysicalServerRepository physicalServerPaymentRepository,
            IPaymentGameServerRepository gameServerPaymentRepository)
        {
            this._unitOfWork = unitOfWork;
            this._billingTransactionRepository = billingRepo;
            this._creditPaymentOrderRepository = creditPaymentOrderRepo;
            _paymentSystemRepository = paymentSystemRepository;
            _discountService = discountService;
            _bilingService = bilingService;
            this._webHostingPaymentRepo = webHostingPaymentRepo;
            this._fixedSubscriptionPaymentRepository = fixedSubscriptionPaymentRepository;
            this._usageSubscriptionPaymentRepository = usageSubscriptionPaymentRepository;
            this._subscriptionBackupPaymentRepository = subscriptionBackupPaymentRepository;
            this._physicalServerPaymentRepository = physicalServerPaymentRepository;
            this._gameServerPaymentRepository = gameServerPaymentRepository;
        }

        public Payment CreateCreditPaymentOrder(decimal cashAmount, string userId, Guid paymentSystem)
        {
            var psystem = _paymentSystemRepository.GetById(paymentSystem);
            if (psystem == null)
            {
                throw new InvalidIdentifierException($"Payment System with id = {paymentSystem} doesn't exist.");
            }
            if (!psystem.IsEnabled)
            {
                throw new InvalidIdentifierException($"Payment System with id = {paymentSystem} is disable.");
            }
            var newOrder = new Payment
            {
                Status = PaymentStatus.Created,
                CashAmount = cashAmount,
                UserId = userId,
                PaymentSystemId = paymentSystem,
                Date = DateTime.UtcNow
            };

            _creditPaymentOrderRepository.Add(newOrder);
            _unitOfWork.Commit();

            return newOrder;
        }

        public void ConfirmCreditPaymentOrder(Guid id, decimal cashAmount)
        {
            var payment = _creditPaymentOrderRepository.GetById(id);
            if (payment == null)
            {
                throw new InvalidIdentifierException($"CreditPaymentOrder with id = {id} doesn't exist.");
            }
            payment.AmountReal = cashAmount;
            payment.AmountWithBonus = _discountService.GetBonusReplenishmentDiscount(cashAmount);
            payment.Status = PaymentStatus.Success;
            _creditPaymentOrderRepository.Update(payment);
            _unitOfWork.Commit();
            
            var transaction = new BillingTransaction
            {
                CashAmount = payment.AmountWithBonus,
                TransactionType = BillingTransactionType.BalanceReplenishment,
                UserId = payment.UserId
            };
            _bilingService.AddUserTransaction(transaction);
        }

        public void FailCreditPaymentOrder(Guid id)
        {
            var payment = _creditPaymentOrderRepository.GetById(id);
            if (payment == null)
            {
                throw new InvalidIdentifierException($"CreditPaymentOrder with id = {id} doesn't exist.");
            }
            payment.Status = PaymentStatus.Failed;
            _creditPaymentOrderRepository.Update(payment);
            _unitOfWork.Commit();
        }


        public void DeleteCreditPaymentOrderById(Guid id)
        {
            var orderToDelete = this.GetCreditPaymentOrderById(id);

            this._creditPaymentOrderRepository.Delete(orderToDelete);
            this._unitOfWork.Commit();
        }


        public virtual Payment GetCreditPaymentOrderById(Guid guid)
        {
            var order = this._creditPaymentOrderRepository.GetById(guid);
            if (order == null)
            {
                throw new InvalidIdentifierException(string.Format("CreditPaymentOrder with id = {0} doesn't exist.", guid.ToString()));
            }

            return order;
        }


        public IPagedList<Payment> GetPage(int pageNumber, int pageSize, SearchPaymentParams filter = null)
        {
            Expression<Func<Payment, bool>> where = x => true;

            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.UserId))
                {
                    where = where.And(p => p.UserId == filter.UserId);
                }

                if (filter.Status != null)
                {
                    where = where.And(p => p.Status == filter.Status);
                }

                if (filter.DateType != null)
                {

                    if (filter.DateType == DateType.StartTransaction)
                    {
                        where = where.And(x => x.Date >= filter.FromDate && x.Date <= filter.ToDate);
                    }
                    if (filter.DateType == DateType.EndTramsaction)
                    {
                        where = where.And(x => x.DateEnd >= filter.FromDate && x.DateEnd <= filter.ToDate);
                    }
                }
                if (filter.PaymentSystemId != null)
                {
                    where = where.And(x => x.PaymentSystemId == filter.PaymentSystemId);
                }
            }

            var page = this._creditPaymentOrderRepository.GetPage(new PageInfo(pageNumber, pageSize), where, (x => x.Date), true,
                x => x.User, x => x.PaymentSystem);

            return page;
        }

        public void EnableDisablePaymentSystem(Guid id, bool enable)
        {
            var paymentSystem = _paymentSystemRepository.GetById(id);
            if (paymentSystem == null)
            {
                throw new InvalidIdentifierException($"Payment System with id = {id} doesn't exist.");
            }
            paymentSystem.IsEnabled = enable;
            _paymentSystemRepository.Update(paymentSystem);
            _unitOfWork.Commit();
        }

        public IEnumerable<PaymentSystem> GetPaymentSystems(bool searchEnabled = false)
        {
            Expression<Func<PaymentSystem, bool>> where = x => true;
            if(searchEnabled)
                where = where.And(x => x.IsEnabled);
            var list = _paymentSystemRepository.GetMany(where);
            return list;
        }

        public IPagedList<BillingTransactionInfo> GetUserBillingTransactionInfosPage(string userId, int pageNumber, int pageSize, DateTime? from = null, DateTime? to = null)
        {
            var transactionSearchParams = new BillingSearchParams
            {
                DateFrom = from,
                DateTo = to,
                UserId = userId
            };
            var userTransactions = this._bilingService.GetPageBillingTransaction(pageNumber, pageSize, transactionSearchParams);

            var paymentInfos = userTransactions.Select(t => new BillingTransactionInfo {BillingTransaction = t, Payments = null }).ToList();

            var webHostingTransactions = userTransactions.Where(t => t.TransactionType == BillingTransactionType.WebHostingPayment);
            IEnumerable<PaymentBase> webHostingPayments = this.GetPaymentsByTransactions<WebHostingPayment>(webHostingTransactions, this._webHostingPaymentRepo);

            var fixedSubscriptionTransactions = userTransactions.Where(t => t.TransactionType == BillingTransactionType.FixedSubscriptionVmPayment);
            IEnumerable<PaymentBase> fixedSubscriptionPayments = this.GetPaymentsByTransactions(fixedSubscriptionTransactions, this._fixedSubscriptionPaymentRepository);
            IEnumerable<PaymentBase> fixedSubscriptionBackupPayments = this.GetPaymentsByTransactions(fixedSubscriptionTransactions, this._subscriptionBackupPaymentRepository);

            var usageSubscriptionTransactions = userTransactions.Where(t => t.TransactionType == BillingTransactionType.UsageSubscriptionVmPayment);
            IEnumerable<PaymentBase> usageSubscriptionPayments = this.GetPaymentsByTransactions(usageSubscriptionTransactions, this._usageSubscriptionPaymentRepository);
            IEnumerable<PaymentBase> usageSubscriptionBackupPayments = this.GetPaymentsByTransactions(usageSubscriptionTransactions, this._subscriptionBackupPaymentRepository);

            var gameServerTransactions = userTransactions.Where(t => t.TransactionType == BillingTransactionType.GameServer);
            IEnumerable<PaymentBase> gameServerPayments = this.GetPaymentsByTransactions(gameServerTransactions, this._gameServerPaymentRepository);

            var physicalServerTransactions = userTransactions.Where(t => t.TransactionType == BillingTransactionType.PhysicalServerPayment);
            IEnumerable<PaymentBase> physicalServerPayments = this.GetPaymentsByTransactions(physicalServerTransactions, this._physicalServerPaymentRepository);

            IEnumerable<PaymentBase> allPayments = webHostingPayments
                .Union(fixedSubscriptionPayments)
                .Union(fixedSubscriptionBackupPayments)
                .Union(usageSubscriptionPayments)
                .Union(usageSubscriptionBackupPayments)
                .Union(gameServerPayments)
                .Union(physicalServerPayments);

            var allPaymentsGroupedByTransaction = allPayments.GroupBy(p => p.BillingTransactionId);
            foreach(var group in allPaymentsGroupedByTransaction)
            {
                var paymentInfo = paymentInfos.Single(pi => pi.BillingTransaction.Id == group.Key);
                paymentInfo.Payments = group.ToArray();
            }

            var staticPagedList = new StaticPagedList<BillingTransactionInfo>(paymentInfos, userTransactions);

            return staticPagedList;
        }

        private IEnumerable<T> GetPaymentsByTransactions<T>(IEnumerable<BillingTransaction> transactions, IRepository<T> repo) where T : PaymentBase
        {
            Expression<Func<T, bool>> paymentWhereExpression = x => false;
            foreach (var transaction in transactions)
            {
                paymentWhereExpression = paymentWhereExpression.Or(p => p.BillingTransactionId == transaction.Id);
            }

            var payments = repo.GetMany(paymentWhereExpression, this.GetRequiredPropIncludes<T>());

            return payments;
        }

        private Expression<Func<T, object>>[] GetRequiredPropIncludes<T>() where T : PaymentBase
        {
            Expression<Func<T, object>>[] includes = new Expression<Func<T, object>>[0];

            if(typeof(T) == typeof(FixedSubscriptionPayment))
            {
                includes = new Expression<Func<T, object>>[4];
                includes[0] = x => (x as FixedSubscriptionPayment).SubscriptionVm;
                includes[1] = x => (x as FixedSubscriptionPayment).SubscriptionVm.UserVm;
                includes[2] = x => (x as FixedSubscriptionPayment).SubscriptionVm.User;
                includes[3] = x => (x as FixedSubscriptionPayment).SubscriptionVm.Tariff;
            }
            if (typeof(T) == typeof(UsageSubscriptionPayment))
            {
                includes = new Expression<Func<T, object>>[3];
                includes[0] = x => (x as UsageSubscriptionPayment).SubscriptionVm;
                includes[1] = x => (x as UsageSubscriptionPayment).SubscriptionVm.UserVm;
                includes[2] = x => (x as UsageSubscriptionPayment).SubscriptionVm.User;
            }
            if (typeof(T) == typeof(WebHostingPayment))
            {
                includes = new Expression<Func<T, object>>[1];
                includes[0] = x => (x as WebHostingPayment).WebHosting;
            }
            if (typeof(T) == typeof(PaymentGameServer))
            {
                includes = new Expression<Func<T, object>>[1];
                includes[0] = x => (x as PaymentGameServer).GameServer;
            }

            return includes;
        }
    }
}
