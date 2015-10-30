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

        public Guid CreateVm(CreateVmTask task)
        {
            Thread.Sleep(10000);

            if (ConfigurationManager.AppSettings["StatusTask"] == "EndWithError")
            {
                throw new CreateVmException("Don't create VM");
            }
            return Guid.NewGuid();
        }


        public void UpdateVm(UpdateVmTask updateVmTask)
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
    }
}
