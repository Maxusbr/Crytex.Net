using System;
using Crytex.Virtualization.Base;
using Crytex.Virtualization.Base.InfoAboutVM;
using Crytex.Virtualization.Base.VMModify;
using System.Collections.Generic;

namespace Crytex.Virtulization.Fake
{
    public class FakeVMachine : IVMachine
    {
        private FakeVmSummary _summary;

        public FakeVMachine(VMModifySpecification vmSpec)
        {
            this._summary = new FakeVmSummary
            {
                BaseInformation = new BaseInfo(vmSpec.MachineName, "", "", "")
            };
        }

        public BaseInfo BaseInformation
        {
            get
            {
                return this._summary.BaseInformation;
            }
        }

        public GuestOSState GuestOSState { get; private set; }

        public INetworkInformation Networks
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public PowerState PowerStateMachine { get; private set; }

        public ProviderVirtualization typeOfProvider
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDrivesInformation VirtualDrives
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ReturnedRezultes CloneMachine(string newMachineName)
        {
            throw new NotImplementedException();
        }

        public ReturnedRezultes CloneMachine(string newMachineName, object specification)
        {
            throw new NotImplementedException();
        }

        public ReturnedRezultes CloneMachine(string newPath, VMModifySpecification ModSpec)
        {
            throw new NotImplementedException();
        }

        public ReturnedRezultes DeleteMachine()
        {
            throw new NotImplementedException();
        }

        public ReturnedRezultes Start(bool WaitForLoadOS)
        {
            this.PowerStateMachine = PowerState.PowerOn;
            this.GuestOSState = Crytex.Virtualization.Base.GuestOSState.Running;

            return new ReturnedRezultes();
        }

        public ReturnedRezultes Stop()
        {
            this.PowerStateMachine = PowerState.PowerOff;
            this.GuestOSState = Crytex.Virtualization.Base.GuestOSState.NotRunning;

            return new ReturnedRezultes();
        }
    }
}