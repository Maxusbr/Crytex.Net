using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Model.Models
{
    public class StateMachine : BaseEntity
    {
        public Int32 CpuLoad { get; set; }
        public Int32 RamLoad { get; set; }
        public DateTime Date { get; set; }
        public Guid VmId { get; set; }
    }
}
