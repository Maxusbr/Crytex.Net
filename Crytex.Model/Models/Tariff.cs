using System;

namespace Crytex.Model.Models
{
    public class Tariff: BaseEntity
    {
        public TypeVirtualization Virtualization { get; set; }
        public OperatingSystemFamily OperatingSystem { get; set; }
        public decimal Processor1 { get; set; }
        public decimal RAM512 { get; set; }
        public decimal HDD1 { get; set; }
        public decimal SSD1 { get; set; }
        public decimal Load10Percent { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }        
    }
}
