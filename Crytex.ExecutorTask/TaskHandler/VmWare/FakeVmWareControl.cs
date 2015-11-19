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

        public CreateVmResult CreateVm(TaskV2 task, ServerTemplate template)
        {
            Thread.Sleep(10000);

            if (ConfigurationManager.AppSettings["StatusTask"] == "EndWithError")
            {
                throw new CreateVmException("Don't create VM");
            }

            return new CreateVmResult { MachineGuid = Guid.NewGuid()};
        }


        public void UpdateVm(TaskV2 updateVmTask)
        {
            Thread.Sleep(10000);
            if (ConfigurationManager.AppSettings["StatusTask"] == "EndWithError")
            {
                throw new InvalidIdentifierException(string.Format("Virtual machine with name {0} doesnt exist on this host",
                  "Name"));
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
            Thread.Sleep(10000);
            if (ConfigurationManager.AppSettings["StatusTask"] == "EndWithError")
            {
                throw new InvalidIdentifierException(
                    string.Format("Virtual machine with name {0} doesnt exist on this host",
                        machineName));
            }
        }

        #endregion // Private methods
    }

    internal enum TypeStandartVmTask
    {
        Start,
        Stop,
        Remove
    }
}
