using System;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class GameServerMachineConfigUpdateViewModel
    {
        [Required]
        public string GameServerId { get; set; }
        public int? Cpu { get; set; }
        public int? Ram { get; set; }
    }
}