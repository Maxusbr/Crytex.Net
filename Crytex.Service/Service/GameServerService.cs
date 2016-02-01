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
        private readonly IGameServerRepository _gameServerRepository;
        private readonly ITaskV2Service _taskService;
        private readonly IBilingService _billingService;
        private readonly IPaymentGameServerRepository _paymentGameServerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GameServerService(IGameServerRepository gameServerRepository, ITaskV2Service taskService,
            IGameServerConfigurationRepository gameServerConfRepository, IBilingService billingService,
            IPaymentGameServerRepository paymentGameServerRepository, IUnitOfWork unitOfWork)
        {
            this._gameServerRepository = gameServerRepository;
            this._taskService = taskService;
            this._gameServerConfRepository = gameServerConfRepository;
            this._unitOfWork = unitOfWork;
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
                Hdd = operatingSystem.MinHardDriveSize,
                Ram = operatingSystem.MinRamCount,
                OperatingSystemId = operatingSystem.Id,
                Name = "Game server"
            };
            var newTask = new TaskV2
            {
                StatusTask = StatusTask.Pending,
                TypeTask = TypeTask.CreateVm,
                UserId = server.UserId,
                Virtualization = TypeVirtualization.HyperV
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
            var operatingSystem = gameServerConf.ServerTemplate.OperatingSystem;

            var taskOptions = new CreateVmOptions
            {
                Cpu = options.PaymentType == ServerPaymentType.Slot ? options.SlotCount * operatingSystem.MinCoreCount : options.Cpu,
                Hdd = operatingSystem.MinHardDriveSize,
                Ram = options.PaymentType == ServerPaymentType.Slot ? options.SlotCount * operatingSystem.MinRamCount : options.Ram,
                OperatingSystemId = operatingSystem.Id,
                Name = server.Name
            };
            var newTask = new TaskV2
            {
                StatusTask = StatusTask.Pending,
                TypeTask = TypeTask.CreateVm,
                UserId = server.UserId,
                Virtualization = TypeVirtualization.HyperV
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
            // Create new GameServer
            server.CreateDate = DateTime.UtcNow;
            server.DateExpire = server.CreateDate.AddMonths(options.ExpireMonthCount);

            server = CreateServer(server, options);
            decimal amount = 0;
            switch (options.PaymentType)
            {
                case ServerPaymentType.Slot:
                    amount = this.BuySlotServer(server, options.SlotCount) * options.ExpireMonthCount;
                    break;
                case ServerPaymentType.Configuration:
                    amount = this.BuyConfigurationServer(server, options.Cpu, options.Ram) * options.ExpireMonthCount;
                    break;
            }

            var gameServerVmTransaction = new BillingTransaction
            {
                CashAmount = -amount,
                TransactionType = BillingTransactionType.OneTimeDebiting,
                UserId = options.UserId
            };
            gameServerVmTransaction = this._billingService.AddUserTransaction(gameServerVmTransaction);

            var gameServerPayment = new PaymentGameServer
            {
                BillingTransactionId = gameServerVmTransaction.Id,
                Date = server.CreateDate,
                DateEnd = server.DateExpire,
                GameServerId = server.Id,
                CoreCount = options.Cpu,
                RamCount = options.Ram,
                CashAmount = amount,
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

        private decimal BuySlotServer(GameServer server, int slotCount)
        {
            var serverConf = server.GameServerConfiguration ?? this._gameServerConfRepository.GetById(server.GameServerConfigurationId);
            var total = serverConf.Slot * slotCount;

            return total;
        }

        private decimal BuyConfigurationServer(GameServer server, int cpu, int ram)
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
                    amount = this.BuySlotServer(server, server.SlotCount);
                    break;
                case ServerPaymentType.Configuration:
                    amount = this.BuyConfigurationServer(server, server.Vm.CoreCount, server.Vm.RamCount);
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
                throw new InvalidIdentifierException($"Game server with id={guid.ToString()} doesn't exist");
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
            ProlongateGameServer(gameServerId, monthCount, BillingTransactionType.AutomaticDebiting);
        }

        public void AutoProlongateGameServer(Guid guid)
        {
            try
            {
                ProlongateGameServer(guid, 1, BillingTransactionType.AutomaticDebiting);
            }
            catch (TransactionFailedException)
            {
                UpdateStatusServer(guid, GameServerStatus.WaitForPayment);
            }
        }

        private void ProlongateGameServer(Guid guid, int monthCount, BillingTransactionType transactionType)
        {
            var server = GetById(guid);
            var gameServerPayment = GetGameServerById(guid);
            decimal amount = 0;
            switch (gameServerPayment.PaymentType)
            {
                case ServerPaymentType.Slot:
                    amount = BuySlotServer(server, gameServerPayment.SlotCount );
                    break;
                case ServerPaymentType.Configuration:
                    amount = BuyConfigurationServer(server, gameServerPayment.RamCount, gameServerPayment.CoreCount );
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
            server.DateExpire = server.DateExpire.AddMonths(monthCount);
            _gameServerRepository.Update(server);

            gameServerPayment.MonthCount = monthCount;
            gameServerPayment.DateEnd = server.DateExpire;
            gameServerPayment.CashAmount = totalPrice;
            gameServerPayment.BillingTransaction = gameServerVmTransaction;
            gameServerPayment.BillingTransactionId = gameServerVmTransaction.Id;

            _paymentGameServerRepository.Update(gameServerPayment);
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
                TypeTask = TypeTask.RemoveVm
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
                    UserId = srv.UserId
                };

                this._taskService.CreateTask(task, taskOptions);
            }
        }
    }
}
