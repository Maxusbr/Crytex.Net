using System;

namespace Crytex.Model.Models
{
    public class StateMachine : BaseEntity
    {
        public Int32 CpuLoad { get; set; }
        public long RamLoad { get; set; }
        public TimeSpan UpTime { get; set; }
        public DateTime Date { get; set; }
        public Guid VmId { get; set; }
    }
}
