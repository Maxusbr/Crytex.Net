using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Enums;

namespace Crytex.Service.Model
{
    public class PhysicalServerStateParams
    {
        public Guid ServerId { get; set; }
        public BoughtPhysicalServerStatus State { get; set; }
        public string AdminMessage { get; set; }
        public bool? AutoProlongation { get; set; }
    }
}
