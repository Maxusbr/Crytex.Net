using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Enums;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Model.Models.Biling;
using Crytex.Service.Extension;
using Crytex.Service.IService;
using Crytex.Service.Model;
using PagedList;

namespace Crytex.Service.Service
{
    public class PhysicalServerService : IPhysicalServerService
    {
        private readonly IBoughtPhysicalServerOptionRepository _boughtOptionRepository;
        private readonly IBoughtPhysicalServerRepository _boughtServerRepository;
        private readonly IPhysicalServerOptionRepository _optionRepository;
        private readonly IPhysicalServerOptionsAvailableRepository _availableOptionRepository;
        private readonly IPhysicalServerRepository _serverRepository;
        private readonly IBilingService _billingService;
        private readonly IUnitOfWork _uniOfWork;

        public PhysicalServerService(IBoughtPhysicalServerOptionRepository boughtOptionRepository,
            IBoughtPhysicalServerRepository boughtServerRepository, IPhysicalServerOptionRepository optionRepository,
            IPhysicalServerOptionsAvailableRepository availableOptionRepository, IPhysicalServerRepository serverRepository,
            IBilingService billingService, IUnitOfWork uniOfWork)
        {
            _boughtOptionRepository = boughtOptionRepository;
            _boughtServerRepository = boughtServerRepository;
            _optionRepository = optionRepository;
            _availableOptionRepository = availableOptionRepository;
            _serverRepository = serverRepository;
            _uniOfWork = uniOfWork;
            _billingService = billingService;
        }

        /// <summary>
        /// Создание конфигурации физического сервера
        /// </summary>
        /// <param name="serverParam"></param>
        /// <returns></returns>
        public PhysicalServer CreatePhysicalServer(CreatePhysicalServerParam serverParam)
        {
            var server = new PhysicalServer { ProcessorName = serverParam.ProcessorName, Description = serverParam.Description };
            if (serverParam.CalculatePrice && serverParam.ServerOptions != null)
                server.Price = serverParam.ServerOptions.Where(o => o.IsDefault).Sum(s => s.Price);
            else
                server.Price = serverParam.Price;
            _serverRepository.Add(server);
            _uniOfWork.Commit();

            if (serverParam.ServerOptions != null && serverParam.ServerOptions.Any())
            {
                CreateOrUpdateOptions(serverParam.ServerOptions);
                AddOptionsAviable(server.Id, serverParam.ServerOptions.Select(o => new OptionAviable { OptionId = o.Id, IsDefault = o.IsDefault }));
            }
            return server;
        }

        /// <summary>
        /// Обновление доступных опций для физического сервера. 
        /// </summary>
        /// <param name="optionsParams"></param>
        public void UpdateOptionsAviable(PhysicalServerOptionsAviableParams optionsParams)
        {
            var server = _serverRepository.GetById(optionsParams.ServerId);
            if (server == null)
            {
                throw new InvalidIdentifierException($"PhysicalServer with id={optionsParams.ServerId} doesn't exist");
            }
            if (optionsParams.ReplaceAll)
                _availableOptionRepository.Delete(x => x.PhysicalServerId == optionsParams.ServerId);
            AddOptionsAviable(optionsParams.ServerId, optionsParams.Options);
        }

        /// <summary>
        /// Добавление доступных опций для физического сервера. 
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="options"></param>
        private void AddOptionsAviable(Guid serverId, IEnumerable<OptionAviable> options)
        {
            var server = _serverRepository.GetById(serverId);
            if (server == null)
            {
                throw new InvalidIdentifierException($"PhysicalServer with id={serverId} doesn't exist");
            }
            foreach (var opt in options)
            {
                var option = _optionRepository.Get(x => x.Id == opt.OptionId);
                if (option == null)
                {
                    throw new InvalidIdentifierException($"PhysicalServerOption with id={opt.OptionId} doesn't exist");
                }
                var aviable = _availableOptionRepository.Get(x => x.PhysicalServerId == serverId && x.OptionId == option.Id,
                        x => x.Option, x => x.Server);
                if (aviable == null)
                    _availableOptionRepository.Add(new PhysicalServerOptionsAvailable
                    {
                        OptionId = option.Id,
                        PhysicalServerId = serverId,
                        IsDefault = opt.IsDefault
                    });
                else
                {
                    aviable.IsDefault = opt.IsDefault;
                    _availableOptionRepository.Update(aviable);
                }
            }
            _uniOfWork.Commit();
        }

        /// <summary>
        /// Создание/изменение опции для физических серверов
        /// </summary>
        /// <param name="optionsParams"></param>
        /// <returns></returns>
        public PhysicalServerOption CreateOrUpdateOption(PhysicalServerOptionsParams optionsParams)
        {
            PhysicalServerOption option;
            if (optionsParams.Id != null)
            {
                option = _optionRepository.Get(x => x.Id == optionsParams.Id);
                if (option == null)
                {
                    throw new InvalidIdentifierException($"PhysicalServerOption with id={optionsParams.Id} doesn't exist");
                }
                option.Description = optionsParams.Description;
                option.Name = optionsParams.Name;
                option.Price = optionsParams.Price;
                option.Type = optionsParams.Type;
                _optionRepository.Update(option);
            }
            else
            {
                option = new PhysicalServerOption
                {
                    Name = optionsParams.Name,
                    Description = optionsParams.Description,
                    Price = optionsParams.Price,
                    Type = optionsParams.Type
                };
                _optionRepository.Add(option);
            }
            _uniOfWork.Commit();
            return option;
        }

        /// <summary>
        /// Создание/изменение опций для физических серверов
        /// </summary>
        /// <param name="optionsParams"></param>
        public void CreateOrUpdateOptions(IEnumerable<PhysicalServerOptionsParams> optionsParams)
        {
            foreach (var el in optionsParams)
                CreateOrUpdateOption(el);
        }

        /// <summary>
        /// Удаление конфигурации физического сервера
        /// </summary>
        /// <param name="serverId"></param>
        public void DeletePhysicalServer(Guid serverId)
        {
            var server = _serverRepository.GetById(serverId);
            if (server == null)
            {
                throw new InvalidIdentifierException($"PhysicalServer with id={serverId} doesn't exist");
            }
            _serverRepository.Delete(server);
            _uniOfWork.Commit();
        }

        /// <summary>
        /// Удаление опции для физического сервера
        /// </summary>
        /// <param name="optionId"></param>
        public void DeletePhysicalServerOption(Guid optionId)
        {
            var option = _optionRepository.Get(x => x.Id == optionId);
            if (option == null)
            {
                throw new InvalidIdentifierException($"PhysicalServerOption with id={optionId} doesn't exist");
            }
            _optionRepository.Delete(option);
            _uniOfWork.Commit();
        }

        public void DeleteBoughtPhysicalServer(Guid serverId)
        {
            var server = _boughtServerRepository.GetById(serverId);
            if (server.Status == BoughtPhysicalServerStatus.Deleted)
            {
                throw new InvalidOperationApplicationException("Subscription is already deleted");
            }
            server.Status = BoughtPhysicalServerStatus.Deleted;
            _boughtServerRepository.Update(server);
            _uniOfWork.Commit();
            if (server.DateEnd > DateTime.UtcNow)
                ReturnMoney(server);
        }

        private void ReturnMoney(BoughtPhysicalServer server, string Description = "Return money for deleted physical server")
        {
            var payments = _boughtServerRepository.GetMany(p => p.Id == server.Id);

            // Find all unspended payments and current payment
            // All unspended payments which are not "active" yet (it's startdate is grater than now) will be returned completely
            var futurePayments = payments.Where(p => p.CreateDate > DateTime.UtcNow);
            // Find current payment (it's startdate is less and enddate is grater than now)
            // This payment will be returned partially
            var currentPayment = payments.SingleOrDefault(p => p.CreateDate <= DateTime.UtcNow && p.DateEnd > DateTime.UtcNow);

            decimal sumToReturn = 0;
            sumToReturn = futurePayments.Sum(p => p.CashAmaunt);
            if (currentPayment != null)
            {
                // Calculate part of current payment to return
                var totalDays = (currentPayment.DateEnd - currentPayment.CreateDate).Days;
                var pastDays = (DateTime.UtcNow - currentPayment.CreateDate).Days;
                var futureDays = totalDays - pastDays;
                sumToReturn += (currentPayment.CashAmaunt / totalDays) * futureDays;
            }

            var transaction = new BillingTransaction
            {
                SubscriptionVmId = server.Id,
                CashAmount = sumToReturn,
                TransactionType = BillingTransactionType.Crediting,
                UserId = server.UserId,
                Description = Description
            };
            this._billingService.AddUserTransaction(transaction);
        }

        /// <summary>
        /// Покупка физического сервера
        /// </summary>
        /// <param name="serverParam"></param>
        /// <returns></returns>
        public BoughtPhysicalServer BuyPhysicalServer(BuyPhysicalServerParam serverParam)
        {
            var serverConfig = _serverRepository.GetById(serverParam.PhysicalServerId);
            if (serverConfig == null)
            {
                throw new InvalidIdentifierException($"PhysicalServer with id={serverParam.PhysicalServerId} doesn't exist");
            }
            var amaunt = GetPriceOption(serverParam.OptionIds);
            var psTransaction = new BillingTransaction
            {
                CashAmount = -amaunt,
                TransactionType = BillingTransactionType.OneTimeDebiting,
                SubscriptionVmMonthCount = serverParam.CountMonth,
                UserId = serverParam.UserId
            };
            var status = BoughtPhysicalServerStatus.New;
            try
            {
                psTransaction = _billingService.AddUserTransaction(psTransaction);
            }
            catch (TransactionFailedException)
            {
                status = BoughtPhysicalServerStatus.WaitPayment;
            }
            var dt = serverParam.CreateDate ?? DateTime.UtcNow;
            var server = new BoughtPhysicalServer
            {
                PhysicalServerId = serverConfig.Id,
                CreateDate = dt,
                DateEnd = dt.AddMonths(serverParam.CountMonth),
                Status = status,
                CountMonth = serverParam.CountMonth,
                UserId = serverParam.UserId,
                BillingTransactionId = psTransaction.Id,
                CashAmaunt = amaunt
            };

            if (serverParam.DiscountPrice != null)
                server.DiscountPrice = serverParam.DiscountPrice ?? 0;

            _boughtServerRepository.Add(server);
            _uniOfWork.Commit();

            AddOptionToBoughtPhysicalServer(server.Id, serverParam.OptionIds);

            return GetBoughtPhysicalServer(server.Id);
        }

        private decimal GetPriceOption(IEnumerable<Guid> options)
        {
            decimal summ = 0;
            foreach (var guid in options)
            {
                var option = _optionRepository.GetById(guid);
                if (option == null)
                {
                    throw new InvalidIdentifierException($"PhysicalServerOption with id={guid} doesn't exist");
                }
                summ += option.Price;
            }
            return summ;
        }

        private void AddOptionToBoughtPhysicalServer(Guid serverId, IEnumerable<Guid> options)
        {
            foreach (var guid in options)
            {
                var option = _optionRepository.GetById(guid);
                if (option == null)
                {
                    throw new InvalidIdentifierException($"PhysicalServerOption with id={guid} doesn't exist");
                }
                var boughtOption = new BoughtPhysicalServerOption { BoughtPhysicalServerId = serverId, OptionId = option.Id };
                _boughtOptionRepository.Add(boughtOption);
            }
            _uniOfWork.Commit();
        }

        /// <summary>
        /// Обновление состояния купленного физического сервера
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="state"></param>
        public void UpdateBoughtPhysicalServerState(PhysicalServerStateParams serverParam)
        {
            var server = _boughtServerRepository.GetById(serverParam.ServerId);
            if (server == null)
            {
                throw new InvalidIdentifierException($"PhysicalServer with id={serverParam.ServerId} doesn't exist");
            }
            if (serverParam.State == BoughtPhysicalServerStatus.Created)
            {
                server.AdminMessage = serverParam.AdminMessage;
                server.AdminSendMessage = true;
            }
            if (serverParam.State == BoughtPhysicalServerStatus.Active)
            {
                server.CreateDate = DateTime.Now;
                server.DateEnd = server.CreateDate.AddMonths(server.CountMonth);
                server.AdminMessage = serverParam.AdminMessage;
                server.AdminSendMessage = true;
            }
            if (serverParam.State == BoughtPhysicalServerStatus.DontCreate)
            {
                server.AdminMessage = serverParam.AdminMessage;
                server.AdminSendMessage = true;
                ReturnMoney(server);
            }
            if (serverParam.State == BoughtPhysicalServerStatus.WaitPayment)
            {
                server.DateEnd = DateTime.UtcNow;
            }
            if (serverParam.State == BoughtPhysicalServerStatus.WaitForDeletion)
            {
                server.DateEnd = DateTime.UtcNow;
            }
            if (serverParam.AutoProlongation != null)
                server.AutoProlongation = serverParam.AutoProlongation ?? false;
            server.Status = serverParam.State;
            _boughtServerRepository.Update(server);
            _uniOfWork.Commit();
        }

        /// <summary>
        /// Обновление конфигурации купленного физического сервера
        /// </summary>
        /// <param name="serverParam"></param>
        /// <returns></returns>
        public BoughtPhysicalServer UpdateBoughtPhysicalServer(UpdatePhysicalServerParam serverParam)
        {
            var server = _boughtServerRepository.Get(x => x.Id == serverParam.ServerId, x => x.ServerOptions,
                x => x.Server, x => x.User);
            if (server == null)
            {
                throw new InvalidIdentifierException($"PhysicalServer with id={serverParam.ServerId} doesn't exist");
            }
            if (serverParam.CountMonth != null)
                server.CountMonth = (int)serverParam.CountMonth;
            if (serverParam.PhysicalServerId != null)
                server.PhysicalServerId = (Guid)serverParam.PhysicalServerId;
            if (serverParam.DiscountPrice != null)
                server.DiscountPrice = (decimal)serverParam.DiscountPrice;
            if (serverParam.OptionIds != null)
            {
                _boughtOptionRepository.Delete(x => x.BoughtPhysicalServerId == server.Id);
                AddOptionToBoughtPhysicalServer(server.Id, serverParam.OptionIds);
            }
            if (serverParam.State != null)
                server.Status = (BoughtPhysicalServerStatus)serverParam.State;
            _boughtServerRepository.Update(server);
            _uniOfWork.Commit();

            return GetBoughtPhysicalServer(server.Id);
        }

        public IPagedList<PhysicalServer> GetPagePhysicalServer(int pageNumber, int pageSize, PhysicalServerSearchParams searchParams = null)
        {
            var pageInfo = new PageInfo(pageNumber, pageSize);
            Expression<Func<PhysicalServer, bool>> where = x => true;


            var pagedList = _serverRepository.GetPage(pageInfo, where, x => x.Id, false);
            foreach (var server in pagedList)
            {
                var options = _availableOptionRepository.GetMany(x => x.PhysicalServerId == server.Id, x => x.Option);
                server.Config = server.ProcessorName;
                if (!options.Any()) continue;
                server.AvailableOptions = options.Where(o => o.IsDefault).ToList();
                foreach (var opt in server.AvailableOptions.Where(o => o.IsDefault))
                    server.Config += ", " + opt.Option.Name;
            }
            return pagedList;
        }

        public IPagedList<PhysicalServerOption> GetPagePhysicalServerOption(int pageNumber, int pageSize,
            PhysicalServerOptionSearchParams searchParams = null)
        {
            var pageInfo = new PageInfo(pageNumber, pageSize);
            Expression<Func<PhysicalServerOption, bool>> where = x => true;


            var pagedList = _optionRepository.GetPage(pageInfo, where, x => x.Id, false);

            return pagedList;
        }

        public IPagedList<BoughtPhysicalServer> GetPageBoughtPhysicalServer(int pageNumber, int pageSize,
            BoughtPhysicalServerSearchParams searchParams = null)
        {
            var pageInfo = new PageInfo(pageNumber, pageSize);
            Expression<Func<BoughtPhysicalServer, bool>> where = x => true;


            var pagedList = _boughtServerRepository.GetPage(pageInfo, where, x => x.Id, false, x => x.Server, x => x.User);
            foreach (var server in pagedList)
            {
                server.Config = server.Server.ProcessorName;
                server.ServerOptions = _boughtOptionRepository.GetMany(x => x.BoughtPhysicalServerId == server.Id,
                    x => x.Option, x => x.Server);
                if (!server.ServerOptions.Any()) continue;
                foreach (var opt in server.ServerOptions)
                    server.Config += ", " + opt.Option.Name;
            }

            return pagedList;
        }

        /// <summary>
        /// Получить конфигурацию готового физического сервера
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public PhysicalServer GetReadyPhysicalServer(Guid serverId)
        {
            var server = _serverRepository.Get(x => x.Id == serverId);
            if (server == null)
            {
                throw new InvalidIdentifierException($"PhysicalServer with id={serverId} doesn't exist");
            }
            var availableOptions = _availableOptionRepository.GetMany(x => x.PhysicalServerId == serverId, x => x.Option)
                .Where(option => option.IsDefault || option.Option.Type == PhysicalServerOptionType.Hdd).ToList();
            server.AvailableOptions = availableOptions;
            return server;
        }

        /// <summary>
        /// Получить конфигурацию физического сервера а также список доступных опций
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public PhysicalServer GetAviablePhysicalServer(Guid serverId)
        {
            var server = _serverRepository.GetById(serverId);
            if (server == null)
            {
                throw new InvalidIdentifierException($"PhysicalServer with id={serverId} doesn't exist");
            }
            server.AvailableOptions = _availableOptionRepository.GetMany(x => x.PhysicalServerId == serverId, x => x.Option);

            return server;
        }

        /// <summary>
        /// Получить конфигурацию купленного физического сервера
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public BoughtPhysicalServer GetBoughtPhysicalServer(Guid serverId)
        {
            var server = _boughtServerRepository.Get(x => x.Id == serverId, x => x.Server, x => x.User);
            if (server == null)
            {
                throw new InvalidIdentifierException($"BoughtPhysicalServer with id={serverId} doesn't exist");
            }

            server.ServerOptions = _boughtOptionRepository.GetMany(x => x.BoughtPhysicalServerId == server.Id,
                x => x.Option, x => x.Server);
            server.Config = server.Server.ProcessorName;
            if (server.ServerOptions.Any())
                foreach (var opt in server.ServerOptions)
                    server.Config += ", " + opt.Option.Name;

            return server;
        }

        public List<BoughtPhysicalServer> GetPhysicalServerByStatus(BoughtPhysicalServerStatus status)
        {
            return _boughtServerRepository.GetMany(x => x.Status == status);
        }

        public List<BoughtPhysicalServer> GetAllUsagePhysicalServer()
        {
            return _boughtServerRepository.GetMany(x => x.Status == BoughtPhysicalServerStatus.Active || 
                x.Status != BoughtPhysicalServerStatus.WaitForDeletion || x.Status != BoughtPhysicalServerStatus.WaitForDeletion);
        }

        public void AutoProlongatePhysicalServer(Guid serverId)
        {
            try
            {
                ProlongatePhysicalServer(serverId, 1, BillingTransactionType.AutomaticDebiting);
            }
            catch (TransactionFailedException)
            {
                UpdateBoughtPhysicalServerState(new PhysicalServerStateParams { ServerId = serverId, State = BoughtPhysicalServerStatus.WaitPayment });
            }
        }

        public List<BoughtPhysicalServer> GetPhysicalServerMessageSend()
        {
            return _boughtServerRepository.GetMany(x => x.AdminSendMessage);
        }

        public void CompleteSendMessage(Guid serverId)
        {
            var server = _boughtServerRepository.GetById(serverId);
            if (server == null)
            {
                throw new InvalidIdentifierException($"PhysicalServer with id={serverId} doesn't exist");
            }
            server.AdminSendMessage = false;
            _boughtServerRepository.Update(server);
            _uniOfWork.Commit();
        }

        private void ProlongatePhysicalServer(Guid serverId, int monthCount, BillingTransactionType transactionType)
        {
            var server = _boughtServerRepository.Get(x => x.Id == serverId, x => x.BillingTransaction, x => x.Server);
            decimal amount = 0;

            var totalPrice = server.Server.Price * monthCount;
            var gameServerVmTransaction = new BillingTransaction
            {
                CashAmount = -totalPrice,
                TransactionType = transactionType,
                UserId = server.UserId,
                SubscriptionVmMonthCount = monthCount
            };
            server.BillingTransaction = this._billingService.AddUserTransaction(gameServerVmTransaction);
            server.CountMonth = monthCount;
            server.DateEnd = DateTime.UtcNow.AddMonths(monthCount);
            server.CashAmaunt = totalPrice;

            _boughtServerRepository.Update(server);
            _uniOfWork.Commit();
        }
    }
}
