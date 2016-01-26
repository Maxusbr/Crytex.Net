namespace Crytex.Model.Models.WebHosting
{
    public class WebHostingTariff : GuidBaseEntity
    {
        public int StorageSizeGB { get; set; }
        public int DomainCount { get; set; }
        public int FtpAccountCount { get; set; }
        public int DatabaseCount { get; set; }
        public decimal Price { get; set; }
    }
}
