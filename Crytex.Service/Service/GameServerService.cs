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
using Crytex.Service.Extension;
using Crytex.Service.Model;

namespace Crytex.Service.Service
{
    class GameServerService : IGameServerService
    {
        private readonly IGameServerConfigurationRepository _gameServerConfRepository;
        private readonly IServerTemplateRepository _serverTemplateRepository;
        private readonly IGameServerRepository _gameServerRepository;
        private readonly ITaskV2Service _taskService;
        private readonly IBilingService _billingService;
        private readonly IPaymentGameServerRepository _paymentGameServerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GameServerService(IGameServerRepository gameServerRepository, ITaskV2Service taskService,
            IGameServerConfigurationRepository gameServerConfRepository, IBilingService billingService,
            IPaymentGameServerRepository paymentGameServerRepository, IServerTemplateRepository serverTemplateRepository, IUnitOfWork unitOfWork)
        {
            this._gameServerRepository = gameServerRepository;
            this._taskService = taskService;
            this._gameServerConfRepository = gameServerConfRepository;
            this._unitOfWork = unitOfWork;
            _serverTemplateRepository = serverTemplateRepository;
            _paymentGameServerRepository = paymentGameServerRepository;
            _billingService = billingService;
        }

        public GameServer CreateServer(GameServer server)
        {
            var gameServerConf = this._gameServerConfRepository.Get(conf => conf.Id == server.GameServerConfigurationId, conf => conf.ServerTemplate.OperatingSystem);
            var operatingSystem = gameServerConf.ServerTemplate.OperatingSystem;

            var taskOptions = new CreateVmOptions
            {
                Cpu = operatingSystem.MinCoreCount,
                HddGB = operatingSystem.MinHardDriveSize,
                Ram = operatingSystem.MinRamCount,
                OperatingSystemId = operatingSystem.Id,
                Name = "Game server"
            };
            var newTask = new TaskV2
            {
                StatusTask = StatusTask.Pending,
                TypeTask = TypeTask.CreateVm,
                UserId = server.UserId,
                Virtualization = TypeVirtualization.HyperV,
                ResourceId = server.Id,
                ResourceType = ResourceType.GameServer
            };

            newTask = this._taskService.CreateTask<CreateVmOptions>(newTask, taskOptions);
            server.VmId = newTask.GetOptions<CreateVmOptions>().UserVmId;

            this._gameServerRepository.Add(server);

            this._unitOfWork.Commit();

            return server;
        }
        public GameServer CreateServer(GameServer server, BuyGameServerOption options)
        {
            var gameServerConf = this._gameServerConfRepository.Get(conf => conf.Id == server.GameServerConfigurationId, conf => conf.ServerTemplate.OperatingSystem);
            if (gameServerConf == null)
            {
                throw new InvalidIdentifierException($"GameServerConfiguration with id={server.GameServerConfigurationId} doesn't exist");
            }
            var operatingSystem = gameServerConf.ServerTemplate.OperatingSystem;
            if (options.PaymentType == ServerPaymentType.Slot && options.SlotCount < 1)
            {
                throw new InvalidIdentifierException($"Cannot create vm with SlotCount={options.SlotCount}.");
            }
            var taskOptions = new CreateVmOptions
            {
                Cpu = options.PaymentType == ServerPaymentType.Slot ? options.SlotCount * operatingSystem.MinCoreCount : options.Cpu,
                HddGB = operatingSystem.MinHardDriveSize,
                Ram = options.PaymentType == ServerPaymentType.Slot ? options.SlotCount * operatingSystem.MinRamCount : options.Ram,
                OperatingSystemId = operatingSystem.Id,
                Name = server.Name
            };
            var newTask = new TaskV2
            {
                StatusTask = StatusTask.Pending,
                TypeTask = TypeTask.CreateVm,
                UserId = server.UserId,
                Virtualization = TypeVirtualization.HyperV,
                ResourceId = server.Id,
                ResourceType = ResourceType.GameServer
            };

            newTask = this._taskService.CreateTask<CreateVmOptions>(newTask, taskOptions);
            server.VmId = newTask.GetOptions<CreateVmOptions>().UserVmId;

            this._gameServerRepository.Add(server);

            this._unitOfWork.Commit();

            return server;
        }

        public virtual GameServer GetById(Guid guid)
        {
            var server = this._gameServerRepository.Get(x => x.Id == guid, x => x.User, x => x.Vm, x => x.GameServerConfiguration);

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

            var pagedList = this._gameServerRepository.GetPage(pageInfo, where, x => x.Id, false, x => x.User, x => x.Vm);

            return pagedList;
        }

        public IEnumerable<GameServer> GetAllByUserId(string userId)
        {
            var servers = this._gameServerRepository.GetMany(s => s.UserId == userId);

            return servers;
        }

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
            server = CreateServer(server, options);
            decimal amount = 0;
            switch (options.PaymentType)
            {
                case ServerPaymentType.Slot:
                    amount = this.GetSlotServerMonthPrice(server, options.SlotCount) * options.ExpireMonthCount;
                    break;
                case ServerPaymentType.Configuration:
                    amount = this.GetConfigurationServerMonthPrice(server, options.Cpu, options.Ram) * options.ExpireMonthCount;
                    break;
            }

            var gameServerVmTransaction = new BillingTransaction
            {
                CashAmount = -amount,
                TransactionType = BillingTransactionType.GameServer,
                UserId = options.UserId
            };
            gameServerVmTransaction = this._billingService.AddUserTransaction(gameServerVmTransaction);

            var gameServerPayment = new PaymentGameServer
            {
                BillingTransactionId = gameServerVmTransaction.Id,
                Date = dateNow,
                DateStart = dateNow,
                DateEnd = server.DateExpire,
                GameServerId = server.Id,
                CoreCount = options.Cpu,
                RamCount = options.Ram,
                Amount = amount,
                UserId = server.UserId,
                SlotCount = options.SlotCount,
                PaymentType = options.PaymentType,
                MonthCount = options.ExpireMonthCount,
                Status = GameServerStatus.Active
            };
            this._paymentGameServerRepository.Add(gameServerPayment);
            this._unitOfWork.Commit();

            return server;
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

            var page = this._paymentGameServerRepository.GetPage(new PageInfo(pageNumber, pageSize), where, (x => x.Date), true, x => x.User);

            return page;
        }

        private decimal GetSlotServerMonthPrice(GameServer server, int slotCount)
        {
            var serverConf = server.GameServerConfiguration ?? this._gameServerConfRepository.GetById(server.GameServerConfigurationId);
            var total = serverConf.Slot * slotCount;

            return total;
        }

        private decimal GetConfigurationServerMonthPrice(GameServer server, int cpu, int ram)
        {
            var serverConf = server.GameServerConfiguration ?? this._gameServerConfRepository.GetById(server.GameServerConfigurationId);
            var total = serverConf.Processor1 * cpu + serverConf.RAM512 * ram;

            return total;
        }

        public decimal GetGameServerMonthPrice(GameServer server)
        {
            decimal amount = 0;
            switch (server.PaymentType)
            {
                case ServerPaymentType.Slot:
                    amount = this.GetSlotServerMonthPrice(server, server.SlotCount);
                    break;
                case ServerPaymentType.Configuration:
                    amount = this.GetConfigurationServerMonthPrice(server, server.Vm.CoreCount, server.Vm.RamCount);
                    break;
            }

            return amount;
        }

        public IEnumerable<PaymentGameServer> GetAllGameServers()
        {
            var subs = this._paymentGameServerRepository.GetAll();
            return subs;
        }

        public IEnumerable<PaymentGameServer> GetGameServerByStatus(GameServerStatus status)
        {
            var subs = this._paymentGameServerRepository.GetMany(s => s.Status == status, s => s.User);
            return subs;
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

        public void UpdateGameServer(Guid serverId, GameServerConfigOptions options)
        {
            switch (options.UpdateType)
            {
                case GameServerUpdateType.Configuration:
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

        private void UpdateNameGameServerConfiguration(Guid gameServerId, GameServerConfigOptions options)
        {
            var srv = GetById(gameServerId);
            srv.Name = options.ServerName;
            _gameServerRepository.Update(srv);
            _unitOfWork.Commit();
        }

        private void EnableProlongateGameServer(Guid gameServerId, bool autoProlongation)
        {
            var gamesrv = GetGameServerById(gameServerId);
            gamesrv.AutoProlongation = autoProlongation;
            _paymentGameServerRepository.Update(gamesrv);
            _unitOfWork.Commit();
        }

        private void ProlongateGameServerMonth(Guid gameServerId, int monthCount)
        {
            ProlongateGameServer(gameServerId, monthCount, BillingTransactionType.GameServer);
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

        private void ProlongateGameServer(Guid guid, int monthCount, BillingTransactionType transactionType)
        {
            var dateNow = DateTime.UtcNow;
            var server = this.GetById(guid);

            decimal amount = 0;
            switch (server.PaymentType)
            {
                case ServerPaymentType.Slot:
                    amount = GetSlotServerMonthPrice(server, server.SlotCount );
                    break;
                case ServerPaymentType.Configuration:
                    amount = GetConfigurationServerMonthPrice(server, server.Vm.RamCount, server.Vm.CoreCount );
                    break;
            }
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

        public void UpdateStatusServer(Guid guid, GameServerStatus status)
        {
            var gameserv = GetGameServerById(guid);
            gameserv.Status = status;
            _paymentGameServerRepository.Update(gameserv);
            _unitOfWork.Commit();
        }

        public void DeleteGameServer(Guid guid)
        {
            var srv = GetById(guid);
            var removeVmOptions = new RemoveVmOptions
            {
                VmId = srv.Vm.Id
            };
            var deleteTask = new TaskV2
            {
                Virtualization = srv.Vm.VirtualizationType,
                UserId = srv.UserId,
                TypeTask = TypeTask.RemoveVm,
                ResourceId = srv.Id,
                ResourceType = ResourceType.GameServer
            };
            this._taskService.CreateTask(deleteTask, removeVmOptions);

            var gameserv = GetGameServerById(guid);
            gameserv.Status = GameServerStatus.Deleted;
            _paymentGameServerRepository.Update(gameserv);
            _unitOfWork.Commit();
        }

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

        public GameServerConfiguration CreateGameServerConfiguration(GameServerConfiguration config)
        {
            var serverTemplate = _serverTemplateRepository.GetById(config.ServerTemplateId);
            if (serverTemplate == null)
            {
                throw new InvalidIdentifierException($"ServerTemplate with id={config.ServerTemplateId} doesn't exist");
            }
            _gameServerConfRepository.Add(config);
            _unitOfWork.Commit();
            return config;
        }

        public void UpdateGameServerConfiguration(GameServerConfiguration config)
        {
            var serverConfig = _gameServerConfRepository.GetById(config.Id);
            if (serverConfig == null)
            {
                throw new InvalidIdentifierException($"GameServerConfiguration with id={config.Id} doesn't exist");
            }
            var serverTemplate = _serverTemplateRepository.GetById(config.ServerTemplateId);
            if (serverTemplate == null)
            {
                throw new InvalidIdentifierException($"ServerTemplate with id={config.ServerTemplateId} doesn't exist");
            }
            serverConfig.GameName = config.GameName;
            serverConfig.ServerTemplateId = config.ServerTemplateId;
            serverConfig.Processor1 = config.Processor1;
            serverConfig.RAM512 = config.RAM512;
            serverConfig.Slot = config.Slot;
            _gameServerConfRepository.Update(serverConfig);
            _unitOfWork.Commit();
        }

        public IEnumerable<GameServerConfiguration> GetGameServerConfigurations()
        {
            return _gameServerConfRepository.GetAll();
        }

        private void ChangeGameServerMachineState(Guid serverId, TypeChangeStatus status)
        {

            var gameserv = GetGameServerById(serverId);
            if (gameserv.Status != GameServerStatus.Active)
            {
                throw new InvalidOperationApplicationException("Cannot start GameServer. GameServer status is not Active");
            }
            else
            {
                var srv = GetById(serverId);
                var taskOptions = new ChangeStatusOptions
                {
                    TypeChangeStatus = status,
                    VmId = srv.Vm.Id
                };
                var task = new TaskV2
                {
                    TypeTask = TypeTask.ChangeStatus,
                    Virtualization = srv.Vm.VirtualizationType,
                    UserId = srv.UserId,
                    ResourceId = srv.Id,
                    ResourceType = ResourceType.GameServer
                };

                this._taskService.CreateTask(task, taskOptions);
            }
        }

        public void UpdateGameServerMachineConfig(Guid gameServerId, UpdateMachineConfigOptions updateOptions)
        {
            var gameServer = this.GetById(gameServerId);

            switch (gameServer.PaymentType)
            {
                case ServerPaymentType.Configuration:
                    this.UpdateConfigurationGameServerMachineConfig(gameServer, updateOptions);
                    break;
                case ServerPaymentType.Slot:
                    throw new TaskOperationException("Updating slot payment type servers is not supported yet.");
            }
        }

        private void UpdateConfigurationGameServerMachineConfig(GameServer gameServer, UpdateMachineConfigOptions updateOptions)
        {
            var dateNow = DateTime.UtcNow;

            if (gameServer.DateExpire < dateNow)
            {
                throw new TaskOperationException("Updating expired gameserver config is not supported");
            }

            // Calclulate remaining time price at the OLD price
            var timeToEnd = gameServer.DateExpire - dateNow;
            var oldConfigMonthPrice = this.GetConfigurationServerMonthPrice(gameServer, gameServer.Vm.CoreCount, gameServer.Vm.RamCount);
            var timeToEndOldPrice = (timeToEnd.Ticks / (decimal)TimeSpan.FromDays(30).Ticks) * oldConfigMonthPrice;

            // Calclulate remaining time price at the OLD price
            var newCpu = updateOptions.Cpu ?? gameServer.Vm.CoreCount;
            var newRam = updateOptions.Ram ?? gameServer.Vm.RamCount;
            var newConfigMonthPrice = this.GetConfigurationServerMonthPrice(gameServer, newCpu, newRam);
            var timeToEndNewPrice = (timeToEnd.Ticks / (decimal)TimeSpan.FromDays(30).Ticks) * newConfigMonthPrice;

            // Do transaction
            var transactionCash = timeToEndNewPrice - timeToEndOldPrice;
            var transaction = new BillingTransaction
            {
                CashAmount = -transactionCash,
                TransactionType = BillingTransactionType.GameServer,
                UserId = gameServer.UserId,
                Description = "Update gameserver vm configuration"
            };
            transaction = this._billingService.AddUserTransaction(transaction);

            // Create update vm task (TaskV2)
            this.CreateUpdateGameServerTask(gameServer, updateOptions);

            // Mark current and subsequent gameserver payments as ReturnedToUser
            var paymentsToMark = this._paymentGameServerRepository.GetMany(p => p.GameServerId == gameServer.Id && p.DateEnd > dateNow);
            foreach (var payment in paymentsToMark)
            {
                payment.ReturnDate = dateNow;
                payment.ReturnedToUser = true;
            }

            // Add new game server payment
            var gameServerPayment = new PaymentGameServer
            {
                BillingTransactionId = transaction.Id,
                Date = dateNow,
                DateStart = dateNow,
                DateEnd = gameServer.DateExpire,
                GameServerId = gameServer.Id,
                CoreCount = updateOptions.Cpu ?? gameServer.Vm.CoreCount,
                RamCount = updateOptions.Ram ?? gameServer.Vm.RamCount,
                Amount = transactionCash > 0 ? transactionCash : 0,
                UserId = gameServer.UserId,
                SlotCount = 0,
                PaymentType = gameServer.PaymentType,
                MonthCount = 0,
                Status = GameServerStatus.Active
            };
            this._paymentGameServerRepository.Add(gameServerPayment);
            this._unitOfWork.Commit();
        }

        private void CreateUpdateGameServerTask(GameServer server, UpdateMachineConfigOptions options)
        {
            var updateTask = new TaskV2
            {
                TypeTask = TypeTask.UpdateVm,
                UserId = server.UserId,
                Virtualization = server.Vm.VirtualizationType,
                ResourceId = server.Id,
                ResourceType = ResourceType.GameServer
            };
            var taskUpdateOptions = new UpdateVmOptions
            {
                Cpu = options.Cpu ?? server.Vm.CoreCount,
                HddGB = server.Vm.HardDriveSize,
                Ram = options.Ram ?? server.Vm.RamCount,
                Name = server.Vm.Name,
                VmId = server.VmId
            };
            this._taskService.CreateTask(updateTask, taskUpdateOptions);
        }
    }
}
