using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Virtualization.Base;
using Crytex.Virtualization.Base.VMModify;

namespace Crytex.Virtulization.Fake
{
    public class FakeProvider: IProviderVM
    {
        public ProviderVirtualization CurrentProvider { get; }
        public ReturnedRezultes ConnectToServer()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public List<IVMachine> GetAllMachines()
        {
            throw new NotImplementedException();
        }

        public IVMachine GetMachinesByName(string machineName)
        {
            throw new NotImplementedException();
        }

        public ReturnedRezultes GetNetworkSwithes()
        {
            throw new NotImplementedException();
        }

        public ReturnedRezultes CreateMachine(VMModifySpecification spec)
        {
            throw new NotImplementedException();
        }
    }
}
