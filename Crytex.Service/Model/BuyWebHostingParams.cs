using System;

namespace Crytex.Service.Model
{
    public class BuyWebHostingParams
    {
        public Guid WebHostingTariffId { get; set; }
        public string Name { get; set; }
        public bool AutoProlongation { get; set; }
        public string UserId { get; set; }
        public int MonthCount { get; set; }
    }
}
