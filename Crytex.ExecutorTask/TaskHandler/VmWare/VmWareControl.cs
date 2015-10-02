using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            this._vmWareProvider.CreateVm(machineGuid.ToString(), task.Cpu, task.Ram);

            return machineGuid;
        }

        private void ConnectIfNotConnected()
        {
            if (this._vmWareProvider.ConnectionState != ConnectionState.Connected)
            {
                this._vmWareProvider.Connect();
            }
        }
    }
}
