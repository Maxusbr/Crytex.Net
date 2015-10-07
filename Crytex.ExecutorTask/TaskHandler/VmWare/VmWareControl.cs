using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VmWareRemote.Exceptions;
using VmWareRemote.Implementations;
using VmWareRemote.Interface;

namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public class VmWareControl : IVmWareControl
    {
        private IVmWareProvider _vmWareProvider;

        public VmWareControl(IVmWareProvider vmWareProvider)
        {
            this._vmWareProvider = vmWareProvider;
        }

        public Guid CreateVm(CreateVmTask task)
        {
            // Connect to server
            ConnectIfNotConnected();
            
            // Generate unique server machine-name by generating new Guid
            var machineGuid = Guid.NewGuid();

            // Create new vm
            var ramMB = task.Ram * 1024;
            this._vmWareProvider.CreateVm(machineGuid.ToString(), task.Cpu, ramMB);

            return machineGuid;
        }


        public void UpdateVm(UpdateVmTask updateVmTask)
        {
            // Connect to server
            ConnectIfNotConnected();

            try
            {
                // Get server machine-name from machine Id
                var machineName = updateVmTask.VmId.ToString();

                // Calculate Ram size in megabytes
                var ramMB = updateVmTask.Ram * 1024;

                // Update vm conf by calling provider's ModifyMachine
                this._vmWareProvider.ModifyMachine(machineName, updateVmTask.Cpu, ramMB);
            }
            // Handle invalid name exceptions
            catch (InvalidNameException e)
            {
                throw new InvalidIdentifierException("Invalid name. See inner exception", e);
            }
        }

        public void StartVm(string machineName)
        {
            this.StandartOperationInner(machineName, TypeStandartVmTask.Start);
        }

        public void StopVm(string machineName)
        {
            this.StandartOperationInner(machineName, TypeStandartVmTask.Stop);
        }

        public void RemoveVm(string machineName)
        {
            this.StandartOperationInner(machineName, TypeStandartVmTask.Remove);
        }

        #region Private methods

        private void StandartOperationInner(string machineName, TypeStandartVmTask typeStandartVmTask)
        {
            // Connect to server
            ConnectIfNotConnected();

            try
            {
                switch (typeStandartVmTask)
                {
                    case TypeStandartVmTask.Start:
                        this._vmWareProvider.StartMachine(machineName);
                        break;
                    case TypeStandartVmTask.Stop:
                        this._vmWareProvider.StopMachine(machineName);
                        break;
                    case TypeStandartVmTask.Remove:
                        this._vmWareProvider.DeleteMachine(machineName);
                        break;
                }
            }
            // Handle invalid name exceptions
            catch (InvalidNameException e)
            {
                throw new InvalidIdentifierException("Invalid name. See inner exception", e);
            }
        }
        
        private void ConnectIfNotConnected()
        {
            if (this._vmWareProvider.ConnectionState != ConnectionState.Connected)
            {
                this._vmWareProvider.Connect();
            }
        }
        #endregion // Private methods
    }
}
