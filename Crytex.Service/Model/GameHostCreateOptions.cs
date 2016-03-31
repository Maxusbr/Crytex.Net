using System;

namespace Crytex.Service.Model
{
    public class GameHostCreateOptions
    {
        public string ServerAddress { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int[] SupportedGamesIds { get; set; }
        public int GameServersMaxCount { get; set; }
        public Guid LocationId { get; set; }
    }
}