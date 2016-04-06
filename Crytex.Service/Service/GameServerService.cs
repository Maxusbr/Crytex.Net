using System;
using System.Collections.Generic;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Data.IRepository;
using Crytex.Data.Infrastructure;
using Crytex.Model.Exceptions;
using PagedList;
using System.Linq.Expressions;
using Crytex.Model.Enums;
using Crytex.Model.Models.Biling;
using Crytex.Model.Models.GameServers;
using Crytex.Service.Extension;
using Crytex.Service.Model;

namespace Crytex.Service.Service
{
    class GameServerService : IGameServerService
    {
        private readonly IGameServerTariffRepository _gameServerTariffRepository;
        private readonly IGameServerRepository _gameServerRepository;
        private readonly ITaskV2Service _taskService;
        private readonly IBilingService _billingService;
        private readonly IPaymentGameServerRepository _paymentGameServerRepository;
        private readonly IGameHostService _gameHostService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDiscountService _discountService;

        public GameServerService(IGameServerRepository gameServerRepository, ITaskV2Service taskService,
            IGameServerTariffRepository gameServerTariffRepository, IBilingService billingService,
            IPaymentGameServerRepository paymentGameServerRepository, IServerTemplateRepository serverTemplateRepository,
            IGameHostService gameHostService, IUnitOfWork unitOfWork, IDiscountService discountService)
        {
            this._gameServerRepository = gameServerRepository;
            this._taskService = taskService;
            this._gameServerTariffRepository = gameServerTariffRepository;
            this._unitOfWork = unitOfWork;
            _discountService = discountService;
            _paymentGameServerRepository = paymentGameServerRepository;
            _gameHostService = gameHostService;
            _billingService = billingService;
        }

        #region Get operations
        public virtual GameServer GetById(Guid guid)
        {
            var server = this._gameServerRepository.Get(x => x.Id == guid, x => x.User, x => x.GameHost, x => x.GameServerTariff,
                x => x.GameServerTariff.Game, x => x.GameServerTariff.Game.ImageFileDescriptor);

            if (server == null)
            {
                throw new InvalidIdentifierException($"GameServer with id={guid} doesn't exist");
            }

            return server;
        }

        public IPagedList<GameServer> GetPage(int pageNumber, int pageSize, string userId = null)
        {
            var pageInfo = new PageInfo(pageNumber, pageSize);
            Expression<Func<GameServer, bool>> where = x => true;

            if (userId != null)
            {
                where = where.And(x => x.UserId == userId);
            }

            var pagedList = this._gameServerRepository.GetPage(pageInfo, where, x => x.Id, false, x => x.User, x => x.GameHost,
                x => x.GameServerTariff, x => x.GameServerTariff.Game, x => x.GameServerTariff.Game.ImageFileDescriptor);

            return pagedList;
        }

        public IEnumerable<GameServer> GetAllByUserId(string userId)
        {
            var servers = this._gameServerRepository.GetMany(s => s.UserId == userId);

            return servers;
        }

        public IPagedList<PaymentGameServer> GetPage(int pageNumber, int pageSize, SearchPaymentGameServerParams filter = null)
        {
            Expression<Func<PaymentGameServer, bool>> where = x => true;

            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.UserId))
                {
                    where = where.And(p => p.UserId == filter.UserId);
                }

                if (filter.DateFrom != null && filter.DateTo != null)
                {
                    where = where.And(x => x.Date >= filter.DateFrom && x.Date <= filter.DateTo);
                }
                if (!string.IsNullOrEmpty(filter.ServerId))
                {
                    where = where.And(x => x.GameServerId.ToString() == filter.ServerId);
                }
            }

            var page = _paymentGameServerRepository.GetPage(new PageInfo(pageNumber, pageSize), where, (x => x.Date), true, x => x.User);

            return page;
        }

        public IEnumerable<GameServer> GetAllGameServers()
        {
            var subs = _gameServerRepository.GetAll();
            return subs;
        }

        public IEnumerable<GameServer> GetGameServerByStatus(GameServerStatus status)
        {
            var subs = _gameServerRepository.GetMany(s => s.Status == status, s => s.User);
            return subs;
        }

        public IEnumerable<GameServerTariff> GetGameServerTariffs()
        {
            return _gameServerTariffRepository.GetAll(x => x.Game, x => x.Game.ImageFileDescriptor);
        }

        public void UpdatePassword(Guid id, string serverNewPassword)
        {
            var server = GetById(id);
            server.Password = serverNewPassword;
            _gameServerRepository.Update(server);
            _unitOfWork.Commit();
        }

        #endregion

        public GameServer BuyGameServer(BuyGameServerOption options)
        {
            var dateNow = DateTime.UtcNow;

            DateTime dateExpire;
            switch (options.CountingPeriodType)
            {
                case CountingPeriodType.Day:
                    dateExpire = dateNow.AddDays(options.ExpirePeriod);
                    break;
                case CountingPeriodType.Month:
                    dateExpire = dateNow.AddMonths(options.ExpirePeriod);
                    break;
                default:
                    throw new ApplicationException($"Unsupported period type {options.CountingPeriodType}");
            }

            // Create new GameServer
            var server = new GameServer
            {
                CreateDate = dateNow,
                DateExpire = dateExpire,
                GameServerTariffId = options.GameServerTariffId,
                AutoProlongation = options.AutoProlongation,
                Name = options.ServerName,
                UserId = options.UserId,
                SlotCount = options.SlotCount
            };
            
            if (options.ExpirePeriod < 1)
            {
                throw new ValidationException("ExpirePeriod must be greater than 0");
            }

            CheckGameHostAvailable(server);

            decimal monthPrice = this.GetSlotServerMonthPrice(server, options.SlotCount);
            decimal amount;

            switch (options.CountingPeriodType)
            {
                case CountingPeriodType.Day:
                    amount = (monthPrice / 30) * options.ExpirePeriod;
                    break;
                case CountingPeriodType.Month:
                    amount = monthPrice * options.ExpirePeriod;
                    break;
                default:
                    throw new ApplicationException($"Unsupported period type {options.CountingPeriodType}");
            }

            decimal discount;
            switch (options.CountingPeriodType)
            {
                case CountingPeriodType.Day:
                    discount = _discountService.GetLongTermDiscountAmount(amount, options.ExpirePeriod / 30,
                        ResourceType.GameServer);
                    break;
                case CountingPeriodType.Month:
                    discount = _discountService.GetLongTermDiscountAmount(amount, options.ExpirePeriod,
                        ResourceType.GameServer);
                    break;
                default:
                    throw new ApplicationException($"Unsupported period type {options.CountingPeriodType}");
            }
            
            decimal amountWithDiscount = amount - discount;

            var gameServerVmTransaction = new BillingTransaction
            {
                CashAmount = -amountWithDiscount,
                TransactionType = BillingTransactionType.GameServer,
                UserId = options.UserId
            };
            gameServerVmTransaction = this._billingService.AddUserTransaction(gameServerVmTransaction);

            server = CreateServer(server, options.GameHostId);

            var gameServerPayment = new PaymentGameServer
            {
                BillingTransactionId = gameServerVmTransaction.Id,
                Date = dateNow,
                DateStart = dateNow,
                DateEnd = server.DateExpire,
                GameServerId = server.Id,
                Amount = amountWithDiscount,
                AmountWithoutDiscounts = amount,
                UserId = server.UserId,
                SlotCount = options.SlotCount,
                ExpirePeriod = options.ExpirePeriod,
                CountingPeriodType = options.CountingPeriodType
            };
            this._paymentGameServerRepository.Add(gameServerPayment);
            this._unitOfWork.Commit();

            return server;
        }

        public void UpdateGameServer(Guid serverId, GameServerConfigOptions options)
        {
            switch (options.UpdateType)
            {
                case GameServerUpdateType.UpdateSettings:
                    UpdateGameServerSettings(serverId, options);
                    break;
                case GameServerUpdateType.Prolongation:
                    ProlongateGameServer(options.ServerId, options.ProlongatePeriod, options.ProlongatePeriodType, BillingTransactionType.GameServer);
                    break;
                case GameServerUpdateType.UpdateSlotCount:
                    UpdateGameServerSlotCount(options.ServerId, options.SlotCount);
                    break;
            }
        }

        public decimal GetGameServerMonthPrice(GameServer server)
        {
            decimal amount = this.GetSlotServerMonthPrice(server, server.SlotCount);

            return amount;
        }

        public void AutoProlongateGameServer(Guid guid)
        {
            try
            {
                ProlongateGameServer(guid, 1, CountingPeriodType.Month, BillingTransactionType.GameServer);
            }
            catch (TransactionFailedException)
            {
                UpdateStatusServer(guid, GameServerStatus.WaitForPayment);
            }
        }

        public void UpdateStatusServer(Guid guid, GameServerStatus status)
        {
            var gameserv = GetById(guid);
            gameserv.Status = status;
            _gameServerRepository.Update(gameserv);
            _unitOfWork.Commit();
        }

        public void UpdateServerState(Guid gameServerId, GameServerState newState)
        {
            var server = GetById(gameServerId);
            server.ServerState = newState;
            _gameServerRepository.Update(server);
            _unitOfWork.Commit();
        }

        public void DeleteGameServer(Guid guid)
        {
            // Create delete server task
            var srv = GetById(guid);
            var removeVmOptions = new DeleteGameServerOptions
            {
                GameServerId = guid
            };
            var deleteTask = new TaskV2
            {
                UserId = srv.UserId,
                TypeTask = TypeTask.DeleteGameServer,
                ResourceType = ResourceType.GameServer,
                ResourceId = srv.Id
            };
            this._taskService.CreateTask(deleteTask, removeVmOptions);

            var dateNow = DateTime.UtcNow;
            // Calculate return money
            if (srv.DateExpire > dateNow)
            {
                decimal returnMoneyAmount = 0;

                // .. Get active payments
                var paymentsToMarkReturned = _paymentGameServerRepository.GetMany(p => p.GameServerId == srv.Id && p.DateEnd > dateNow
                    && p.ReturnedToUser == false);
                foreach (var payment in paymentsToMarkReturned)
                {
                    payment.ReturnDate = dateNow;
                    payment.ReturnedToUser = true;
                    // Если дата начала платежа больше текущей то возвращаем платёж полностью. Если нет, то частично
                    if (payment.DateStart < dateNow)
                    {
                        returnMoneyAmount += payment.Amount *
                                             ((payment.DateEnd - dateNow).Value.Ticks /
                                              (decimal)(payment.DateEnd - payment.DateStart).Value.Ticks);
                    }
                    else
                    {
                        returnMoneyAmount += payment.Amount;
                    }
                }

                // Return money
                var returnMoneyTransaction = new BillingTransaction
                {
                    CashAmount = returnMoneyAmount,
                    Date = DateTime.UtcNow,
                    TransactionType = BillingTransactionType.ReturnMoneyForDeletedService,
                    UserId = srv.UserId,
                    Description = "Return money for deleted game server service"
                };
                _billingService.AddUserTransaction(returnMoneyTransaction);
            }

            var gameserv = GetById(guid);
            gameserv.Status = GameServerStatus.Deleted;
            _gameServerRepository.Update(gameserv);
            _unitOfWork.Commit();
        }

        #region Change server status operations
        public void StartGameServer(Guid serverId)
        {
            ChangeGameServerMachineState(serverId, TypeChangeStatus.Start);
        }

        public void StopGameServer(Guid serverId)
        {
            ChangeGameServerMachineState(serverId, TypeChangeStatus.Stop);
        }

        public void PowerOffGameServer(Guid serverId)
        {
            ChangeGameServerMachineState(serverId, TypeChangeStatus.PowerOff);
        }

        public void ResetGameServer(Guid serverId)
        {
            ChangeGameServerMachineState(serverId, TypeChangeStatus.Reload);
        }
        #endregion

        public GameServerTariff CreateGameServerTariff(GameServerTariff tariff)
        {
            tariff.CreateDate = DateTime.UtcNow;

            _gameServerTariffRepository.Add(tariff);
            _unitOfWork.Commit();
            return tariff;
        }
        public void UpdateGameServerTariff(GameServerTariff config)
        {
            var serverConfig = _gameServerTariffRepository.GetById(config.Id);
            if (serverConfig == null)
            {
                throw new InvalidIdentifierException($"GameServerConfiguration with id={config.Id} doesn't exist");
            }

            serverConfig.Slot = config.Slot;
            serverConfig.Name = config.Name;
            serverConfig.Performance = config.Performance;
            serverConfig.Disabled = config.Disabled;

            _gameServerTariffRepository.Update(serverConfig);
            _unitOfWork.Commit();
        }

        public void DeleteGameServerTariff(Int32 id)
        {
            var existingTariff = _gameServerTariffRepository.GetById(id);
            if (existingTariff == null)
            {
                throw new InvalidIdentifierException($"GameServerTariff with id={id} doesn't exist");
            }

            _gameServerTariffRepository.Delete(existingTariff);
            _unitOfWork.Commit();
        }

        #region Private methods
        private void CheckGameHostAvailable(GameServer server)
        {
            var gameServerTariff = this._gameServerTariffRepository.Get(tariff => tariff.Id == server.GameServerTariffId);

            var gameHost = _gameHostService.GetGameHostWithAvalailableSlot(gameServerTariff.GameId);
            if (gameHost == null)
            {
                throw new TaskOperationException("Cannot find gamehost for new gameserver");
            }
        }

        private GameServer CreateServer(GameServer server, int? gameHostId = null)
        {
            var gameServerTariff = this._gameServerTariffRepository.Get(tariff => tariff.Id == server.GameServerTariffId);
            if (gameServerTariff == null)
            {
                throw new InvalidIdentifierException($"GameServerTariff with id={server.GameServerTariffId} doesn't exist");
            }

            if (gameHostId != null)
            {
                var canCreateOnHost = _gameHostService.CanCreateServerOnHost(gameHostId.Value, gameServerTariff.GameId);
                if (!canCreateOnHost)
                {
                    gameHostId = _gameHostService.GetGameHostWithAvalailableSlot(gameServerTariff.GameId).Id;
                }
            }
            else
            {
                gameHostId = _gameHostService.GetGameHostWithAvalailableSlot(gameServerTariff.GameId).Id;
            }
            
            int firstPortInRange = _gameHostService.GetFreePort(gameHostId.Value);

            server.GameHostId = gameHostId.Value;
            server.Status = GameServerStatus.Active;
            server.PortRangeSize = 3;
            server.FirstPortInRange = firstPortInRange;
            server.ServerState = GameServerState.Creating;
            this._gameServerRepository.Add(server);
            this._unitOfWork.Commit();

            var taskOptions = new CreateGameServerOptions
            {
                GameServerId = server.Id,
                SlotCount = server.SlotCount,
                GameServerFirstPortInRange = firstPortInRange
            };
            var newTask = new TaskV2
            {
                StatusTask = StatusTask.Pending,
                TypeTask = TypeTask.CreateGameServer,
                ResourceType = ResourceType.GameServer,
                ResourceId = server.Id,
                UserId = server.UserId
            };

            _taskService.CreateTask<CreateGameServerOptions>(newTask, taskOptions);

            return server;
        }

        private PaymentGameServer GetGameServerById(Guid guid)
        {
            var srv = _paymentGameServerRepository.Get(x => x.GameServerId == guid, x => x.User);
            if (srv == null)
            {
                throw new InvalidIdentifierException($"Game server with id={guid} doesn't exist");
            }
            return srv;
        }
        private void ProlongateGameServer(Guid guid, int prololngatePeriod, CountingPeriodType periodType, BillingTransactionType transactionType)
        {
            var dateNow = DateTime.UtcNow;
            var server = this.GetById(guid);

            decimal monthPrice = GetSlotServerMonthPrice(server, server.SlotCount);

            decimal totalPrice;
            switch (periodType)
            {
                case CountingPeriodType.Day:
                    totalPrice = (monthPrice/30) * prololngatePeriod;
                    break;
                case CountingPeriodType.Month:
                    totalPrice = monthPrice * prololngatePeriod;
                    break;
                default:
                    throw new ApplicationException($"Unsupported period type {periodType}");
            }
            var gameServerVmTransaction = new BillingTransaction
            {
                CashAmount = -totalPrice,
                TransactionType = transactionType,
                GameServerId = guid,
                UserId = server.UserId,
                GameServer = server
            };
            gameServerVmTransaction = this._billingService.AddUserTransaction(gameServerVmTransaction);

            var serverExpired = server.DateExpire < dateNow;
            var serverDateExpireOld = server.DateExpire;

            DateTime dateExpireNew;
            switch (periodType)
            {
                case CountingPeriodType.Day:
                    dateExpireNew = serverExpired ? dateNow.AddDays(prololngatePeriod) : server.DateExpire.AddDays(prololngatePeriod);
                    break;
                case CountingPeriodType.Month:
                    dateExpireNew = serverExpired ? dateNow.AddMonths(prololngatePeriod) : server.DateExpire.AddMonths(prololngatePeriod);
                    break;
                default:
                    throw new ApplicationException($"Unsupported period type {periodType}");
            }

            server.DateExpire = dateExpireNew;
            _gameServerRepository.Update(server);

            var gameServerPayment = new PaymentGameServer
            {
                ExpirePeriod = prololngatePeriod,
                CountingPeriodType = periodType,
                Date = dateNow,
                DateStart = serverExpired ? dateNow : serverDateExpireOld.AddTicks(1),
                DateEnd = server.DateExpire,
                Amount = totalPrice,
                AmountWithoutDiscounts = totalPrice,
                BillingTransactionId = gameServerVmTransaction.Id,
                GameServerId = server.Id
            };

            _paymentGameServerRepository.Add(gameServerPayment);
            _unitOfWork.Commit();
        }
        private void UpdateGameServerSettings(Guid gameServerId, GameServerConfigOptions options)
        {
            var srv = GetById(gameServerId);
            srv.Name = options.ServerName ?? srv.Name;
            srv.AutoProlongation = options.AutoProlongation ?? srv.AutoProlongation;
            _gameServerRepository.Update(srv);
            _unitOfWork.Commit();
        }

        private decimal GetSlotServerMonthPrice(GameServer server, int slotCount)
        {
            var serverTariff = server.GameServerTariff ?? this._gameServerTariffRepository.GetById(server.GameServerTariffId);
            var total = serverTariff.Slot * slotCount;

            return total;
        }

        private void ChangeGameServerMachineState(Guid serverId, TypeChangeStatus status)
        {
            var gameserv = GetById(serverId);
            if (gameserv.Status != GameServerStatus.Active)
            {
                throw new InvalidOperationApplicationException("Cannot start GameServer. GameServer status is not Active");
            }
            else
            {
                var taskOptions = new ChangeGameServerStatusOptions
                {
                    TypeChangeStatus = status,
                    GameServerId = gameserv.Id,
                    GameServerPassword = gameserv.Password,
                    GameServerPort = gameserv.FirstPortInRange
                };
                var task = new TaskV2
                {
                    TypeTask = TypeTask.GameServerChangeStatus,
                    UserId = gameserv.UserId,
                    ResourceType = ResourceType.GameServer,
                    ResourceId = gameserv.Id
                };
                this._taskService.CreateTask(task, taskOptions);
            }
        }
        private void UpdateGameServerSlotCount(Guid serverId, int newSlotCount)
        {
            var server = GetById(serverId);

            if (server.SlotCount == newSlotCount)
            {
                throw new ConfigNotChangedException("Current server slot count is same as provided new value");
            }

            var dateNow = DateTime.UtcNow;

            if (server.DateExpire < dateNow)
            {
                throw new TaskOperationException("Updating expired gameserver config is not supported");
            }

            var daysToServerExpire = (server.DateExpire - dateNow).Days;
            var serverDayOldPrice = GetGameServerMonthPrice(server)/30;
            var daysToEndOldCost = serverDayOldPrice*daysToServerExpire;

            var oldSlotCount = server.SlotCount;

            server.SlotCount = newSlotCount;
            var serverDayNewPrice = GetGameServerMonthPrice(server)/30;
            var daysToEndNewCost = serverDayNewPrice*daysToServerExpire;

            var billingTransactionCashAmount = -(daysToEndNewCost - daysToEndOldCost);
            var gameServerTransaction = new BillingTransaction
            {
                CashAmount = billingTransactionCashAmount,
                TransactionType = BillingTransactionType.FixedSubscriptionVmPayment,
                UserId = server.UserId,
                Description = "Update game server slot count"
            };

            try
            {
                gameServerTransaction = this._billingService.AddUserTransaction(gameServerTransaction);
            }
            catch (TransactionFailedException)
            {
                // Restore UserVm ef entity state
                server.SlotCount = oldSlotCount;
                throw;
            }

            // Create update gameserver task
            this.UpdateGameServerConfig(server, newSlotCount);

            // Update game server
            _gameServerRepository.Update(server);

            // Mark current and subsequent gameserver payments as ReturnedToUser
            var paymentsToMark = _paymentGameServerRepository.GetMany(p => p.GameServerId == server.Id && p.DateEnd > dateNow 
                && p.ReturnedToUser == false);
            foreach (var payment in paymentsToMark)
            {
                payment.ReturnDate = dateNow;
                payment.ReturnedToUser = true;
            }

            // Add new gameserver payment
            dateNow = DateTime.UtcNow;
            var subscriptionPayment = new PaymentGameServer
            {
                BillingTransactionId = gameServerTransaction.Id,
                Date = DateTime.UtcNow,
                DateStart = dateNow,
                DateEnd = server.DateExpire,
                GameServerId = server.Id,
                Amount = daysToEndNewCost,
                AmountWithoutDiscounts = daysToEndNewCost,
                UserId = server.UserId,
                SlotCount = newSlotCount
            };
            _paymentGameServerRepository.Add(subscriptionPayment);
            this._unitOfWork.Commit();
        }

        private void UpdateGameServerConfig(GameServer server, int newSlotCount)
        {
            var updateTask = new TaskV2
            {
                TypeTask = TypeTask.UpdateGameServer,
                UserId = server.UserId,
                ResourceId = server.Id,
                ResourceType = ResourceType.GameServer
            };
            var taskUpdateOptions = new UpdateGameServerOptions
            {
                GameServerId = server.Id,
                SlotCount = newSlotCount
            };
            this._taskService.CreateTask(updateTask, taskUpdateOptions);
        }

        #endregion
    }
}
