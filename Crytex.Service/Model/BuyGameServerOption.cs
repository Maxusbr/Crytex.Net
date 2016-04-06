using Crytex.Model.Enums;

namespace Crytex.Service.Model
{
    public class BuyGameServerOption
    {
        public int GameServerTariffId { get; set; }
        public int SlotCount { get; set; }
        public int ExpirePeriod { get; set; }
        public CountingPeriodType CountingPeriodType { get; set; }
        public string UserId { get; set; }
        public bool AutoProlongation { get; set; }
        public string ServerName { get; set; }
        public int? GameHostId { get; set; }
    }
}
