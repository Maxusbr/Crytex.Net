using System;
using Crytex.Model.Models;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class GameServerViewModel
    {
        public Guid Id { get; set; }
        [Required]

        public string Name { get; set; }

        public int? GameServerConfigurationId { get; set; }

        [Required]
        public ServerPaymentType? PaymentType { get; set; }

        public int? SlotCount { get; set; }

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public Guid VmId { get; set; }

        public string VmName { get; set; }
        public int Cpu { get; set; }
        public int Ram { get; set; }
        public int ExpireMonthCount { get; set; }
    }
}