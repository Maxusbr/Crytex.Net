using System;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class SubscriptionProlongateOptionsViewModel
    {
        [Required]
        public Guid? SubscriptionVmId { get; set; }
        [Required]
        public int? MonthCount { get; set; }
    }
}