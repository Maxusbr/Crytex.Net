﻿using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class GameServerRepository : RepositoryBase<GameServer>, IGameServerRepository
    {
        public GameServerRepository(IDatabaseFactory dbFactory) : base(dbFactory) { }
    }
}
