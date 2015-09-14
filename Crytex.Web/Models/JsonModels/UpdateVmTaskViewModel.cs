using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crytex.Web.Models.JsonModels
{
    public class UpdateVmTaskViewModel
    {
        public string VmId { get; set; }
        public Int32 Cpu { get; set; }
        public Int32 Ram { get; set; }
        public Int32 Hdd { get; set; }
    }
}
