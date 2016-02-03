using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Enums;

namespace Crytex.Model.Models
{
    public class PhysicalServerOption : GuidBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public PhysicalServerOptionType Type { get; set; }
    }
}
