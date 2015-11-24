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
        [EnumDataType(typeof(TypeVirtualization))]
        public TypeVirtualization Virtualization { get; set; }

        [Required]
        public Double Processor1 { get; set; }

        [Required]
        public Double RAM512 { get; set; }

        [Required]
        public Double HDD1 { get; set; }

        [Required]
        public Double SSD1 { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}