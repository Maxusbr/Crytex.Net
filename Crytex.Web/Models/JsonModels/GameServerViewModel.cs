using System;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class GameServerViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int? GameServerTariffId { get; set; }

        public int GameHostId { get; set; }
        [Required]
        public int? SlotCount { get; set; }

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public int ExpireMonthCount { get; set; }
    }
}