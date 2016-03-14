using System;

namespace Crytex.Background.Monitor.Fake
{
    class FakeVmMonitor : IVmMonitor
    {
        private Random rnd = new Random((int) (DateTime.Now.Ticks%int.MaxValue));

        public VmState GetMachineState(string machineName)
        {


            var state = new VmState
            {
                CpuUsage = 1,
                Uptime = TimeSpan.FromSeconds(rnd.Next(10000)),
                RamUsage = 1,
            };

            return state;
        }
    }
}
