using Crytex.Model.Models;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class GameServerViewModel
    {
        [Required]
        public int? GameServerConfigurationId { get; set; }
        [Required]
        public ServerPaymentType? PaymentType { get; set; }
        [Required]
        public int? SlotCount { get; set; }
    }
}