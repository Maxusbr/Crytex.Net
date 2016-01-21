using System;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Data.IRepository;
using Crytex.Data.Infrastructure;
using Crytex.Model.Exceptions;
using PagedList;
using System.Linq.Expressions;
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
                Ram = options.PaymentType == ServerPaymentType.Slot ? options.SlotCount * operatingSystem.MinRamCount :options.Ram,
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
            var server = this._gameServerRepository.Get(x => x.Id == guid, x => x.User, x => x.Vm);

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
                    amount = this.BuySlotServer(server, options);
                    break;
                case ServerPaymentType.Configuration:
                    amount = this.BuyConfigurationServer(server, options);
                    break;
            }

            var gameServerVmTransaction = new BillingTransaction
            {
                CashAmount = -amount,
                TransactionType = BillingTransactionType.OneTimeDebiting,
                SubscriptionVmMonthCount = options.ExpireMonthCount,
                UserId = options.UserId
            };
            gameServerVmTransaction = this._billingService.AddUserTransaction(gameServerVmTransaction);

            var gameServerPayment = new PaymentGameServer
            {
                BillingTransactionId = gameServerVmTransaction.Id,
                Date = server.CreateDate,
                DateEnd = server.DateExpire,
                VmId = server.Id,
                CoreCount = options.Cpu,
                RamCount = options.Ram,
                CashAmount = amount,
                UserId = server.UserId,
                SlotCount = options.SlotCount,
                PaymentType = options.PaymentType
            };
            this._paymentGameServerRepository.Add(gameServerPayment);
            this._unitOfWork.Commit();

            return server;
        }

        private decimal BuySlotServer(GameServer server, BuyGameServerOption options)
        {
            var themplate = server.GameServerConfiguration;
            var total = themplate.Slot * options.SlotCount;

            return total;
        }

        private decimal BuyConfigurationServer(GameServer server, BuyGameServerOption options)
        {
            var themplate = server.GameServerConfiguration;
            var total = themplate.Processor1 * options.Cpu + themplate.RAM512 * options.Ram;

            return total;
        }
    }
}
