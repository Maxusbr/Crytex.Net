using System;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class SubscriptionProlongateOptionsViewModel
    {
        [Required]
        public Guid? SubscriptionId { get; set; }
        [Required]
        public int? MonthCount { get; set; }
    }
}