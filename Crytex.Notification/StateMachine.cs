using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Notification
{
    public class StateMachine
    {
        public Int32 CpuLoad { get; set; }
        public long RamLoad { get; set; }
        public DateTime Date { get; set; }
    }
}
