using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Crytex.Model.Models;

namespace Crytex.Web.Models.JsonModels
{
    public class TariffViewModel
    {
        [Required]
        public Int32 Id { get; set; }

        [Required]
        [EnumDataType(typeof(TypeOfOperatingSystem))]
        public TypeOfOperatingSystem OperatingSystem { get; set; }

        [Required]
        [EnumDataType(typeof(TypeVirtualization))]
        public TypeVirtualization Virtualization { get; set; }

        [Required]
        public decimal Processor1 { get; set; }

        [Required]
        public decimal RAM512 { get; set; }

        [Required]
        public decimal HDD1 { get; set; }

        [Required]
        public decimal SSD1 { get; set; }

        [Required]
        public decimal Load10Percent { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}