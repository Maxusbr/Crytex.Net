using Crytex.Model.Models;
using Crytex.Model.Models.Biling;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class SubscriptionBuyOptionsUserViewModel
    {
        [Required]
        public bool? AutoProlongation { get; set; }
        [Required]
        public int? Cpu { get; set; }
        [Required]
        public int? Hdd { get; set; }
        [Required]
        public int? OperatingSystemId { get; set; }
        [Required]
        public int? Ram { get; set; }
        [Required]
        public int? SDD { get; set; }
        [Required]
        public int? SubscriptionsMonthCount { get; set; }
        [Required]
        public SubscriptionType? SubscriptionType { get; set; }
        [Required]
        public TypeVirtualization? Virtualization { get; set; }
    }

    public class SubscriptionBuyOptionsAdminViewModel
    {
        [Required]
        public string UserId { get; set; }
    }
}