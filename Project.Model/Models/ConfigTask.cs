using System;


namespace Project.Model.Models
{
    public class ConfigTask :BaseTask
    {
        public Int32 Cpu { get; set; }
        public Int32 Ram { get; set; }
        public Int32 Hdd { get; set; }
        public String Name { get; set; }
    }
}
