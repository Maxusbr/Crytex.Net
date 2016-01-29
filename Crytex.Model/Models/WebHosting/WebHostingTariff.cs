using System;

namespace Crytex.Model.Models.WebHostingModels
{
    public class WebHostingTariff : GuidBaseEntity
    {
        public string Name { get; set; }
        public int StorageSizeGB { get; set; }
        public int DomainCount { get; set; }
        public int FtpAccountCount { get; set; }
        public int DatabaseCount { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
