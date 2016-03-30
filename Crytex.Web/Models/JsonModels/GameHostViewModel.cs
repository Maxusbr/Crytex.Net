using System;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class GameHostViewModel
    {
        public Int32 Id { get; set; }
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
        [Required]
        public Guid? LocationId { get; set; }
        public int[] SupportedGamesIds { get; set; }
    }
}