using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class BonusReplenishmentViewModel
    {
        public int? Id { get; set; }
        [Required]
        public int? UserReplenishmentSize { get; set; }
        [Required]
        public double? BonusSize { get; set; }
        public bool Disable { get; set; }
    }
}