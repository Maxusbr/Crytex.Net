using System;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Service.IService;
using System.Security.Principal;
using Crytex.Model.Models;
using Microsoft.AspNet.Identity;
using Crytex.Model.Exceptions;

namespace Crytex.Service.Service.SecureService
{
    class SecureGameServerService : GameServerService, IGameServerService
    {
        private readonly IIdentity _userIdentity;

        public SecureGameServerService(IGameServerRepository gameServerRepo, ITaskV2Service taskService,
            IGameServerConfigurationRepository gameServerConfRepository, IUnitOfWork unitOfWork,
            IIdentity userIdentity) : base(gameServerRepo, taskService, gameServerConfRepository, unitOfWork)
        {
            this._userIdentity = userIdentity;
        }

        public override GameServer GetById(Guid guid )
        {
            var server = base.GetById(guid);

            if(server.UserId != this._userIdentity.GetUserId())
            {
                throw new SecurityException($"Access denied for GameServer with id={guid}");
            }

            return server;
        }
    }
}
