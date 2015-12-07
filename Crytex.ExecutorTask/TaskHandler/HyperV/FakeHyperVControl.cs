using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using HyperVRemote;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using Crytex.ExecutorTask.TaskHandler.VmWare;

namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public class FakeHyperVControl : IHyperVControl
    {
        private IHyperVProvider _hyperVProvider;

        public FakeHyperVControl(IHyperVProvider hyperVProvider)
        {
            this._hyperVProvider = hyperVProvider;
        }

        public Guid BackupVm(TaskV2 taskEntity)
        {
            throw new NotImplementedException();
        }

        public Guid CreateVm(TaskV2 task, ServerTemplate template)
        {
            Thread.Sleep(10000);
           
            if (ConfigurationManager.AppSettings["StatusTask"] == "EndWithError") {
                
                throw new CreateVmException("Don't create VM");
            }
            return Guid.NewGuid();
        }

        public void RemoveVm(string machineName)
        {
            this.StandartOperationInner(machineName, TypeStandartVmTask.Start);
        }

        public void StartVm(string machineName)
        {
            this.StandartOperationInner(machineName, TypeStandartVmTask.Start);

        }

        public void StopVm(string machineName)
        {
            this.StandartOperationInner(machineName, TypeStandartVmTask.Start);
        }

        private void StandartOperationInner(string machineName, TypeStandartVmTask typeStandartVmTask)
        {
            Thread.Sleep(2000);
            if (ConfigurationManager.AppSettings["StatusTask"] == "EndWithError")
            {
                throw new InvalidIdentifierException(
                    string.Format("Virtual machine with name {0} doesnt exist on this host",
                        machineName));
            }
        }
        public void UpdateVm(TaskV2 updateVmTask)
        {
            Thread.Sleep(10000);
            if (ConfigurationManager.AppSettings["StatusTask"] == "EndWithError")
            {
                throw new InvalidIdentifierException(string.Format("Virtual machine with name {0} doesnt exist on this host",
                  updateVmTask.GetOptions<UpdateVmOptions>().VmId));
            }
        }
    }
}
