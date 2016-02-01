using Crytex.Model.Models;
using Crytex.Model.Models.Biling;

namespace Crytex.Service.Model
{
    public class SubscriptionBuyOptions
    {
        public bool AutoProlongation { get; set; }
        public int Cpu { get; set; }
        public int Hdd { get; set; }
        public int OperatingSystemId { get; set; }
        public int Ram { get; set; }
        public int SDD { get; set; }
        public string VmName { get; set; }
        public int SubscriptionsMonthCount { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public string UserId { get; set; }
        public string AdminUserId { get; set; }
        public bool BoughtByAdmin { get; set; }
        public TypeVirtualization Virtualization { get; set; }
        /// <summary>
        /// Кол-во дней, на протяжении которых будут храниться ежедневные автоматические бэкапы
        /// </summary>
        public int DailyBackupStorePeriodDays { get; set; }
    }
}
