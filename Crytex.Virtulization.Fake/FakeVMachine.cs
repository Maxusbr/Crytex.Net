﻿using System;
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
            get; set;
        }

        public INetworkInformation Networks
        {
            get
            {
                var networks = new NetworkInformation();
                networks.NetAdapter = new System.Collections.Generic.List<NetworkInfo>();
                networks.NetAdapter.Add(new NetworkInfo
                {
                    IPv4 = "192.168.0.1",
                    IPv6 = "2001:0:5ef5:79fd:10a6:2543:3f57:fefd",
                    MAC = "E6-F8-9C-41-98-94",
                    NetworkName = "Test Network 1"
                });
                networks.NetAdapter.Add(new NetworkInfo
                {
                    IPv4 = "192.168.0.2",
                    IPv6 = "2002:0:5ef5:79fd:10a6:2543:3f57:fefd",
                    MAC = "E6-F8-9C-41-98-95",
                    NetworkName = "Test Network 2"
                });

                return networks;
            }
        }

        public int NumCPU
        {
            get; set;
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
            get; set;
        }

        public ReturnedRezultes CloneMachine(string newMachineName, object specification)
        {
            throw new NotImplementedException();
        }

        public ReturnedRezultes CloneMachine(string newPath)
        {
            return new ReturnedRezultes();
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
            return new ReturnedRezultes();
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
    }
}