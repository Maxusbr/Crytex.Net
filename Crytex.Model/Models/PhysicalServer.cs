using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Enums;

namespace Crytex.Model.Models
{
    public class PhysicalServer : GuidBaseEntity
    {
        public string ProcessorName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public virtual string Config { get; set; }
        public virtual ICollection<PhysicalServerOptionsAvailable> AvailableOptions { get; set; }

    }
}
