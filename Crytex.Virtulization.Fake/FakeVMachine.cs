using System;
using Crytex.Virtualization.Base;
using Crytex.Virtualization.Base.InfoAboutVM;

namespace Crytex.Virtualization.Fake
{
    public class FakeVMachine : IVMachine
    {
        private FakeVmSummary _summary;

        public FakeVMachine()
        {

        }

        public BaseInfo BaseInformation
        {
            get
            {
                return this._summary.BaseInformation;
            }
        }

        public GuestOSState GuestOSState { get; private set; }

        public long Memory
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public INetworkInformation Networks
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int NumCPU
        {
            get
            {
                throw new NotImplementedException();
            }

            set
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

        public ReturnedRezultes CloneMachine(string newMachineName, object specification)
        {
            throw new NotImplementedException();
        }

        public ReturnedRezultes CloneMachine(string newPath)
        {
            throw new NotImplementedException();
        }

        public ReturnedRezultes CreateBackup(string backupName)
        {
            return new ReturnedRezultes();
        }

        public ReturnedRezultes DeleteBackup(string backupName)
        {
            return new ReturnedRezultes();
        }

        public ReturnedRezultes DeleteMachine()
        {
            throw new NotImplementedException();
        }

        public ReturnedRezultes GetHardwareSpecification()
        {
            throw new NotImplementedException();
        }

        public ReturnedRezultes Modify()
        {
            throw new NotImplementedException();
        }

        public ReturnedRezultes Reboot()
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

        public ReturnedRezultes CreateSnapshot(string snapshotServerName)
        {
            return new ReturnedRezultes();
        }

        public ReturnedRezultes DeleteSnapshot(string snapshotServerName, bool deleteWithChildrens)
        {
            return new ReturnedRezultes();
        }
    }
}