using System;
using System.Collections.Generic;
using VmWareRemote.Implementations;
using VmWareRemote.Interface;
using VmWareRemote.Model;

namespace Crytex.Background.Monitor.Vmware
{
    class FakeVmWareProvider : IVmWareProvider
    {
        public FakeVmWareProvider()
        {
        }

        public ConnectionState ConnectionState { get; }
        public void Connect()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void CreateVm(string name, int cpu, long ram, int? hdd = null)
        {
            throw new NotImplementedException();
        }

        public void CloneVm(string sourceVmName, string newVmName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VmWareVirtualMachine> GetAllVirtualMachines()
        {
            throw new NotImplementedException();
        }

        public VmWareVirtualMachine GetMachineByName(string vmName)
        {
            var machine = new VmWareVirtualMachine
            {
                Name = "VmWareMachine",
                CpuUsage = 1,
                Uptime = 1,
                RamUsage = 1,
                State = VmPowerState.On
            };

            return machine;
        }

        public void StopMachine(string machineName)
        {
            throw new NotImplementedException();
        }

        public void StartMachine(string machineName)
        {
            throw new NotImplementedException();
        }

        public void ResetMachine(string machineName)
        {
            throw new NotImplementedException();
        }

        public void DeleteMachine(string machineName)
        {
            throw new NotImplementedException();
        }

        public void ModifyMachine(string machineName, int cpu, long ram, int? hdd = null)
        {
            throw new NotImplementedException();
        }

        public void AddHardDisk(string machineName, int capacity)
        {
            throw new NotImplementedException();
        }

        public void AddNetwork(string machineName, string networkName, string mac = null)
        {
            throw new NotImplementedException();
        }

        public void DeleteNetwork(string machineName, string networkName)
        {
            throw new NotImplementedException();
        }


        public void AddExistingHardDisk(string machineName, string path, int? capacity = null)
        {
            throw new NotImplementedException();
        }

        public int RunPowerShellScript(string machineName, string userName, string password, string script)
        {
            throw new NotImplementedException();
    }

        public int RunLinuxShellScript(string machineName, string userName, string password, string script)
        {
            throw new NotImplementedException();
}
    }
}
