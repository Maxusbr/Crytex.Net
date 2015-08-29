using System;

namespace Crytex.Service.Model
{
    public class CreateVmOption
    {
        public Int32 Cpu { get; set; }
        public Int32 Ram { get; set; }
        public Int32 Hdd { get; set; }
        public String Name { get; set; }
        public String UserId { get; set; }
    }
}
