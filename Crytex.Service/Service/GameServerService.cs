using System;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Data.IRepository;
using Crytex.Data.Infrastructure;
using Crytex.Model.Exceptions;
using PagedList;
using System.Linq.Expressions;
using Crytex.Service.Extension;

namespace Crytex.Service.Service
{
    class GameServerService : IGameServerService
    {
        private readonly IGameServerConfigurationRepository _gameServerConfRepository;
        private readonly IGameServerRepository _gameServerRepository;
        private readonly ITaskV2Service _taskService;
        private readonly IUnitOfWork _unitOfWork;

        public GameServerService(IGameServerRepository gameServerRepository, ITaskV2Service taskService,
            IGameServerConfigurationRepository gameServerConfRepository, IUnitOfWork unitOfWork)
        {
            this._gameServerRepository = gameServerRepository;
            this._taskService = taskService;
            this._gameServerConfRepository = gameServerConfRepository;
            this._unitOfWork = unitOfWork;
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
                OperatingSystemId = operatingSystem.Id
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
            var server = this._gameServerRepository.GetById(guid);
            
            if(server == null)
            {
                throw new InvalidIdentifierException($"GameServer with id={guid} doesn't exist");
            }

            return server;
        }

        public IPagedList<GameServer> GetPage(int pageNumber, int pageSize, string userId = null)
        {
            var pageInfo = new PageInfo(pageNumber, pageSize);
            Expression<Func<GameServer, bool>> where = x => true;

            if(userId != null)
            {
                where = where.And(x => x.UserId == userId);
            }

            var pagedList = this._gameServerRepository.GetPage(pageInfo, where, x => x.Id);

            return pagedList;
        }
    }
}
