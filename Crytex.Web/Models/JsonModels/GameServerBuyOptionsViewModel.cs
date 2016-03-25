using System;
using System.ComponentModel.DataAnnotations;
using Crytex.Model.Enums;

namespace Crytex.Web.Models.JsonModels
{
    public class GameServerBuyOptionsViewModel
    {
        [Required]
        public int? GameServerTariffId { get; set; }
        [Required]
        public int? SlotCount { get; set; }
        [Required]
        public int? ExpirePeriod { get; set; }
        [Required]
        public CountingPeriodType? CountingPeriodType { get; set; }
        [Required]
        public string ServerName { get; set; }
        [Required]
        public Int32 GameId { get; set; }
        public string UserId { get; set; }
        public bool? AutoProlongation { get; set; }
    }
}