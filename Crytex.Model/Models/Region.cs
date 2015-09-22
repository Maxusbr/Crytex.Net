using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models
{
    public class Region: BaseEntity
    {
        public string Name { get; set; }
        public bool Enable { get; set; }
        public string Area { get; set; }
    }
}
