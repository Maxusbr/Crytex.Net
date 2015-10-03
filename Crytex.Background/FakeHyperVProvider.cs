using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using HyperVRemote;
using HyperVRemote.Source.Implementation;

namespace Crytex.Background
{
    class FakeHyperVProvider: IHyperVProvider
    {
        HyperVConfiguration Configuration { get; }

        public FakeHyperVProvider(HyperVConfiguration configuration)
        {
            Configuration = configuration;
        }
        public ResultInfo Connect()
        {
            throw new NotImplementedException();
        }

        public ResultInfoConnectVm ConnectToVm(OsType osType, string ipAddress, string login, string password)
        {
            throw new NotImplementedException();
        }

        public IVhdService GetVhdService()
        {
            throw new NotImplementedException();
        }

        public ISnapshotService GetSnapshotService()
        {
            throw new NotImplementedException();
        }

        public INetworkService GetNetworkService()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<HyperVMachine> GetVms()
        {
            throw new NotImplementedException();
        }

        public HyperVMachine GetVmByName(string vmName)
        {
            PSObject name = new PSObject();
            name.Properties.Add(new PSNoteProperty("CPUUsage", 1));
            name.Properties.Add(new PSNoteProperty("MemoryAssigned", Convert.ToInt64(2)));
            name.Properties.Add(new PSNoteProperty("Name", vmName));
            name.Properties.Add(new PSNoteProperty("State", "State"));
            name.Properties.Add(new PSNoteProperty("Status", "Status"));
            name.Properties.Add(new PSNoteProperty("Uptime", TimeSpan.MinValue));

            return new HyperVMachine(name);
        }

        public bool IsVmExist(string vmName)
        {
            throw new NotImplementedException();
        }

        public ResultInfoGetMemory GetMemoryInfo(HyperVMachine machine)
        {
            throw new NotImplementedException();
        }

        public ResultInfo ModifyMemoryVm(HyperVMachine machine, bool isDynamic, uint minimum, uint maximum, uint startup)
        {
            throw new NotImplementedException();
        }

        public ResultInfoGetProcessor GetProcessorInfo(HyperVMachine machine)
        {
            throw new NotImplementedException();
        }

        public ResultInfo ModifyProcessorVm(HyperVMachine machine, uint count)
        {
            throw new NotImplementedException();
        }

        public ResultInfoCreateVM CreateVm(string nameNewVm, uint memorySize)
        {
            throw new NotImplementedException();
        }

        public ResultInfo RemoveVm(HyperVMachine machine)
        {
            throw new NotImplementedException();
        }

        public ResultInfo Start(HyperVMachine machine)
        {
            throw new NotImplementedException();
        }

        public ResultInfo Stop(HyperVMachine machine, bool isForce = false)
        {
            throw new NotImplementedException();
        }

        public ResultInfo Reset(HyperVMachine machine)
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
