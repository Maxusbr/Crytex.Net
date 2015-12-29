using System;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class MachineConfigUpdateViewModel
    {
        [Required]
        public Guid? SubscriptionId { get; set; }
        public int? Cpu { get; set; }
        public int? Ram { get; set; }
        public int? Hdd { get; set; }
    }
}