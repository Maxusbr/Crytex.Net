using Crytex.Model.Models;
using Crytex.Model.Models.Biling;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class SubscriptionBuyOptionsUserViewModel
    {
        [Required]
        public int? Cpu { get; set; }
        [Required]
        public int? OperatingSystemId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int? Ram { get; set; }
        public int? SSD { get; set; }
        [Required]
        public int? Hdd { get; set; }
        public int? SubscriptionsMonthCount { get; set; }
        public bool? AutoProlongation { get; set; }
        [Required]
        public SubscriptionType? SubscriptionType { get; set; }
        [Required]
        public TypeVirtualization? Virtualization { get; set; }
        /// <summary>
        /// Кол-во дней, на протяжении которых будут храниться ежедневные автоматические бэкапы
        /// </summary>
        [Range(2, 30)]
        public int? DailyBackupStorePeriodDays { get; set; }
    }

    public class SubscriptionBuyOptionsAdminViewModel : SubscriptionBuyOptionsUserViewModel
    {
        [Required]
        public string UserId { get; set; }
    }
}