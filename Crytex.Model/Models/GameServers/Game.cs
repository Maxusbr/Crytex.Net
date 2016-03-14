using System.Collections;
using System.Collections.Generic;
using Crytex.Model.Enums;

namespace Crytex.Model.Models.GameServers
{
    public class Game : BaseEntity
    {
        public string Name { get; set; }
        public GameFamily Family { get; set; }
        public string Version { get; set; }
        public string VersionCode { get; set; }
        public ICollection<GameHost> GameHosts { get; set; } 
    }

}
