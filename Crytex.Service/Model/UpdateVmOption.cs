using System;

namespace Crytex.Service.Model
{
    public class UpdateVmOption
    {
        public Int32 VmId { get; set; }
        public Int32 Cpu{ get; set; }
    public Int32 Ram { get; set; }
        public Int32 Hdd { get; set; }
        public String Name { get; set; }
        public String UserId { get; set; }
    }
}
