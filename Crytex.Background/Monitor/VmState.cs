using System;

namespace Crytex.Background.Monitor
{
    public class VmState
    {
        public int CpuUsage { get; set; }
        public long RamUsage { get; set; }
        public TimeSpan Uptime { get; set; }
    }
}
