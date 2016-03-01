﻿using System.Collections;
using System.Collections.Generic;

namespace Crytex.Model.Models.GameServers
{
    public class Game : BaseEntity
    {
        public string Name { get; set; }
        public GameFamily Family { get; set; }
        public string Version { get; set; }
        public ICollection<GameHost> GameHosts { get; set; } 
    }

    public enum GameFamily
    {
        CounterStrike = 0
    }
}