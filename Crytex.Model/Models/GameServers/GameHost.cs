﻿using System.Collections.Generic;

namespace Crytex.Model.Models.GameServers
{
    public class GameHost : BaseEntity
    {
        public string ServerAddress { get; set; }
        public uint Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public ICollection<Game> SupportedGames { get; set; }
    }
}
