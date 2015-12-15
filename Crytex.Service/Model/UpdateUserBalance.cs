using System.ComponentModel.DataAnnotations;

namespace Crytex.Service.Model
{
    public class UpdateUserBalance
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
}