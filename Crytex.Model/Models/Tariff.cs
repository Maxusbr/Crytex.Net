using System;

namespace Crytex.Model.Models
{
    public class Tariff: BaseEntity
    {
        public TypeVirtualization Virtualization { get; set; }
        public Double Processor1 { get; set; }
        public Double RAM512 { get; set; }
        public Double HDD1 { get; set; }
        public Double SSD1 { get; set; }
        public Double Load10Percent { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }        
    }
}
