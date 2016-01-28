using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Enums;

namespace Crytex.Service.Model
{
    public class PhysicalServerOptionsParams
    {
        public Guid? OptionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public PhysicalServerOptionType Type { get; set; }
        public bool IsDefault { get; set; }
    }
}
