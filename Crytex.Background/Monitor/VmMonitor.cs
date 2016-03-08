using System;
using Crytex.Virtualization.Base;

namespace Crytex.Background.Monitor
{
    class VmMonitor : IVmMonitor
    {
        private readonly IProviderVM _virtualizationProvider;

        internal VmMonitor(IProviderVM virtualizationProvider)
        {
            _virtualizationProvider = virtualizationProvider;
        }

        public VmState GetMachineState(string machineName)
        {
            _virtualizationProvider.ConnectToServer();

            var vm = _virtualizationProvider.GetMachinesByName(machineName);
            var vmResourceAlloc = vm.ResourceAllocation;
            var state = new VmState
            {
                CpuUsage = (int) vmResourceAlloc.CPUUsagePersistent,
                RamUsage = vmResourceAlloc.MemoryUsage,
                Uptime = TimeSpan.MinValue, // TODO: Add Uptime
            };

            _virtualizationProvider.Disconnect();

            return state;
        }
    }
}
