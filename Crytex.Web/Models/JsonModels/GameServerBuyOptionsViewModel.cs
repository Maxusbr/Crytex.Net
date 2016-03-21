using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class GameServerBuyOptionsViewModel
    {
        [Required]
        public int? GameServerTariffId { get; set; }
        [Required]
        public int? SlotCount { get; set; }
        [Required]
        public int? ExpireMonthCount { get; set; }
        [Required]
        public string ServerName { get; set; }
        public string UserId { get; set; }
        public bool? AutoProlongation { get; set; }
    }
}