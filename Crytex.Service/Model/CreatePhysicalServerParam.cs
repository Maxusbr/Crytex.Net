using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models;

namespace Crytex.Service.Model
{
    public class CreatePhysicalServerParam
    {
        public string ProcessorName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool CalculatePrice { get; set; }
        public ICollection<PhysicalServerOptionsParams> ServerOptions { get; set; }
    }
}
