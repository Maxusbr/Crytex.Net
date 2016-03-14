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

        public GameServerService(IGameServerRepository gameServerRepository, ITaskV2Service taskService,
            IGameServerTariffRepository gameServerTariffRepository, IBilingService billingService,
            IPaymentGameServerRepository paymentGameServerRepository, IServerTemplateRepository serverTemplateRepository,
            IGameHostService gameHostService, IUnitOfWork unitOfWork)
        {
            this._gameServerRepository = gameServerRepository;
            this._taskService = taskService;
            this._gameServerTariffRepository = gameServerTariffRepository;
            this._unitOfWork = unitOfWork;
            _paymentGameServerRepository = paymentGameServerRepository;
            _gameHostService = gameHostService;
            _billingService = billingService;
        }
        #region Get operations
        public virtual GameServer GetById(Guid guid)
        {
            var server = this._gameServerRepository.Get(x => x.Id == guid, x => x.User, x => x.GameHost, x => x.GameServerTariff, x => x.GameServerTariff.Game);

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

            var pagedList = this._gameServerRepository.GetPage(pageInfo, where, x => x.Id, false, x => x.User, x => x.GameHost);

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

                if (filter.FromDate != null && filter.ToDate != null)
                {
                    where = where.And(x => x.Date >= filter.FromDate && x.Date <= filter.ToDate);
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
            return _gameServerTariffRepository.GetAll();
        }
        #endregion

        public GameServer BuyGameServer(GameServer server, BuyGameServerOption options)
        {
            var dateNow = DateTime.UtcNow;

            // Create new GameServer
            server.CreateDate = dateNow;
            server.DateExpire = dateNow.AddMonths(options.ExpireMonthCount);
            if (options.ExpireMonthCount < 1)
            {
                throw new InvalidIdentifierException("ExpireMonthCount must be greater than 0");
            }

            decimal amount = this.GetSlotServerMonthPrice(server, options.SlotCount) * options.ExpireMonthCount;

            var gameServerVmTransaction = new BillingTransaction
            {
                CashAmount = -amount,
                TransactionType = BillingTransactionType.GameServer,
                UserId = options.UserId
            };
            gameServerVmTransaction = this._billingService.AddUserTransaction(gameServerVmTransaction);

            server = CreateServer(server, options);

            var gameServerPayment = new PaymentGameServer
            {
                BillingTransactionId = gameServerVmTransaction.Id,
                Date = dateNow,
                DateStart = dateNow,
                DateEnd = server.DateExpire,
                GameServerId = server.Id,
                Amount = amount,
                UserId = server.UserId,
                SlotCount = options.SlotCount,
                MonthCount = options.ExpireMonthCount
            };
            this._paymentGameServerRepository.Add(gameServerPayment);
            this._unitOfWork.Commit();

            return server;
        }

        public void UpdateGameServer(Guid serverId, GameServerConfigOptions options)
        {
            switch (options.UpdateType)
            {
                case GameServerUpdateType.UpdateName:
                    UpdateNameGameServerConfiguration(serverId, options);
                    break;
                case GameServerUpdateType.EnableAutoProlongation:
                    EnableProlongateGameServer(serverId, options.AutoProlongation);
                    break;
                case GameServerUpdateType.Prolongation:
                    ProlongateGameServerMonth(serverId, options.MonthCount);
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
                ProlongateGameServer(guid, 1, BillingTransactionType.GameServer);
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

        public void DeleteGameServer(Guid guid)
        {
            //throw new NotImplementedException();
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
            _gameServerTariffRepository.Update(serverConfig);
            _unitOfWork.Commit();
        }
        #region Private methods
        private GameServer CreateServer(GameServer server, BuyGameServerOption options)
        {
            var gameServerTariff = this._gameServerTariffRepository.Get(tariff => tariff.Id == server.GameServerTariffId);
            if (gameServerTariff == null)
            {
                throw new InvalidIdentifierException($"GameServerTariff with id={server.GameServerTariffId} doesn't exist");
            }

            var gameHost = _gameHostService.GetGameHostWithAvalailableSlot(gameServerTariff.GameId);
            if (gameHost == null)
            {
                throw new TaskOperationException("Cannot find gamehost for new gameserver");
            }
            server.GameHostId = gameHost.Id;
            server.Status = GameServerStatus.Active;
            this._gameServerRepository.Add(server);
            this._unitOfWork.Commit();

            var taskOptions = new CreateGameServerOptions
            {
                GameServerId = server.Id,
                GameServerTariffId = server.GameServerTariffId,
                GameHostId = server.GameHostId
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
        private void ProlongateGameServer(Guid guid, int monthCount, BillingTransactionType transactionType)
        {
            var dateNow = DateTime.UtcNow;
            var server = this.GetById(guid);

            decimal amount = GetSlotServerMonthPrice(server, server.SlotCount);

            var totalPrice = amount * monthCount;
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
            server.DateExpire = serverExpired ? dateNow.AddMonths(monthCount) : server.DateExpire.AddMonths(monthCount);
            _gameServerRepository.Update(server);

            var gameServerPayment = new PaymentGameServer
            {
                MonthCount = monthCount,
                Date = dateNow,
                DateStart = serverExpired ? dateNow : serverDateExpireOld.AddTicks(1),
                DateEnd = server.DateExpire,
                Amount = totalPrice,
                BillingTransactionId = gameServerVmTransaction.Id,
                GameServerId = server.Id
            };

            _paymentGameServerRepository.Add(gameServerPayment);
            _unitOfWork.Commit();
        }
        private void UpdateNameGameServerConfiguration(Guid gameServerId, GameServerConfigOptions options)
        {
            var srv = GetById(gameServerId);
            srv.Name = options.ServerName;
            _gameServerRepository.Update(srv);
            _unitOfWork.Commit();
        }

        private void EnableProlongateGameServer(Guid gameServerId, bool autoProlongation)
        {
            var gamesrv = GetById(gameServerId);
            gamesrv.AutoProlongation = autoProlongation;
            _gameServerRepository.Update(gamesrv);
            _unitOfWork.Commit();
        }

        private void ProlongateGameServerMonth(Guid gameServerId, int monthCount)
        {
            ProlongateGameServer(gameServerId, monthCount, BillingTransactionType.GameServer);
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
                    GameServerId = gameserv.Id
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

        private void CreateUpdateGameServerTask(GameServer server, UpdateMachineConfigOptions options)
        {
            throw new NotImplementedException();
            //var updateTask = new TaskV2
            //{
            //    TypeTask = TypeTask.UpdateVm,
            //    UserId = server.UserId,
            //    Virtualization = server.Vm.VirtualizationType
            //};
            //var taskUpdateOptions = new UpdateVmOptions
            //{
            //    Cpu = options.Cpu ?? server.Vm.CoreCount,
            //    HddGB = server.Vm.HardDriveSize,
            //    Ram = options.Ram ?? server.Vm.RamCount,
            //    Name = server.Vm.Name,
            //    VmId = server.Vm.Id
            //};
            //this._taskService.CreateTask(updateTask, taskUpdateOptions);
        }
        #endregion
    }
}
