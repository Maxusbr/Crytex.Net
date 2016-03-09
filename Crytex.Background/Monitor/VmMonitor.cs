using System;
using System.Threading;
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

            while(vm.ResourceAllocation.Update() == false)
            {
                Thread.Sleep(10000);    
            }

            var vmResourceAlloc = vm.ResourceAllocation;
            var state = new VmState
            {
                CpuUsage = (int) (vmResourceAlloc.CPUUsagePersistent * 100),
                RamUsage = vmResourceAlloc.MemoryUsage,
                Uptime = TimeSpan.FromSeconds(10), // TODO: Add Uptime
            };

            _virtualizationProvider.Disconnect();

            return state;
        }
    }
}
