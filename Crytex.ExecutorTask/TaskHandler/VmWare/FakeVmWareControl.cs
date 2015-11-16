using System;
using System.Configuration;
using System.Threading;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using VmWareRemote.Exceptions;
using VmWareRemote.Interface;

namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public class FakeVmWareControl : IVmWareControl
    {
        private IVmWareProvider _vmWareProvider;

        public FakeVmWareControl(IVmWareProvider vmWareProvider)
        {
            this._vmWareProvider = vmWareProvider;
        }

        public Guid CreateVm(TaskV2 task)
        {
            throw new NotImplementedException();
        }

        public void UpdateVm(TaskV2 updateVmTask)
        {
            throw new NotImplementedException();
        }

        public Guid CreateVm(TaskV2 task, ServerTemplate serverTemplate)
        {
            Thread.Sleep(10000);

            if (ConfigurationManager.AppSettings["StatusTask"] == "EndWithError")
            {

                throw new CreateVmException("Don't create VM");
            }
            return Guid.NewGuid();
        }

        public void StartVm(string machineName)
        {
            throw new NotImplementedException();
        }

        public void StopVm(string machineName)
        {
            throw new NotImplementedException();
        }

        public void RemoveVm(string machineName)
        {
            throw new NotImplementedException();
        }
    }
}
