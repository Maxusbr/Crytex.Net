using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class GameHostViewModel
    {
        [Required]
        public string ServerAddress { get; set; }
        [Required]
        public uint? Port { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public uint GameServersMaxCount { get; set; }
        public int[] SupportedGamesIds { get; set; }
    }
}