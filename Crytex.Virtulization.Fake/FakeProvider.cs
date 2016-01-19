using System;
using System.Collections.Generic;
using System.Linq;
using Crytex.Virtualization.Base;
using Crytex.Virtualization.Base.VMModify;

namespace Crytex.Virtulization.Fake
{
    public class FakeProvider: IProviderVM
    {
        private static List<FakeVMachine> _staticMachines;
        private readonly static bool _createFakeEntries;
        static FakeProvider()
        {
            var createFakeMachines = 
            _staticMachines = new List<FakeVMachine>();

            if (FakeProvider._createFakeEntries)
            {
                for (int i = 0; i < 5; i++)
                {
                    var vmSpec = new VMModifySpecification
                    {
                        MachineName = $"AutomaticFakeMachine{i}"
                    };
                    _staticMachines.Add(new FakeVMachine(vmSpec));
                }
            }
        }

        private bool _isConntected = false;

        public ProviderVirtualization CurrentProvider { get; }

        public FakeProvider(ProviderVirtualization virtualizaionType)
        {
            this.CurrentProvider = virtualizaionType;
        }

        public ReturnedRezultes ConnectToServer()
        {
            this._isConntected = true;
            var results = new ReturnedRezultes(false, null, 0);
            return results;
        }

        public void Disconnect()
        {
            this._isConntected = false;
        }

        public List<IVMachine> GetAllMachines()
        {
            this.ThrowExceptionIfNotConnnected();

            return FakeProvider._staticMachines.ConvertAll<IVMachine>(x => (IVMachine)x);
        }

        public IVMachine GetMachinesByName(string machineName)
        {
            this.ThrowExceptionIfNotConnnected();

            return FakeProvider._staticMachines.FirstOrDefault(m => m.BaseInformation.Name == machineName);
        }

        public ReturnedRezultes GetNetworkSwithes()
        {
            this.ThrowExceptionIfNotConnnected();

            throw new NotImplementedException();
        }

        public ReturnedRezultes CreateMachine(VMModifySpecification spec)
        {
            this.ThrowExceptionIfNotConnnected();

            var newFakeMachine = new FakeVMachine(spec);

            FakeProvider._staticMachines.Add(newFakeMachine);

            return new ReturnedRezultes();
        }

        #region Private methods
        public void ThrowExceptionIfNotConnnected()
        {
            if (this._isConntected)
            {
                throw new ApplicationException("You must first connect to server before using provider");
            }
        }
        #endregion
    }
}
