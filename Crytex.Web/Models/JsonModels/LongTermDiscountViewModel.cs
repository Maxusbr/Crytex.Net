using System.ComponentModel.DataAnnotations;
using Crytex.Model.Models;

namespace Crytex.Web.Models.JsonModels
{
    public class LongTermDiscountViewModel
    {
        public int Id { get; set; }
        [Required]
        public int? MonthCount { get; set; }
        [Required]
        public ResourceType? ResourceType { get; set; }
        [Required]
        public double? DiscountSize { get; set; }

        public bool Disable { get; set; }
    }
}