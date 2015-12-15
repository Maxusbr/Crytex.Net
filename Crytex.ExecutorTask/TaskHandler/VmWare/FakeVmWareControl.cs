using System;
using System.Configuration;
using System.Threading;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using VmWareRemote.Exceptions;
using VmWareRemote.Interface;
using OperatingSystem = Crytex.Model.Models.OperatingSystem;

namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public class FakeVmWareControl : IVmWareControl
    {
        private IVmWareProvider _vmWareProvider;

        public FakeVmWareControl(IVmWareProvider vmWareProvider)
        {
            this._vmWareProvider = vmWareProvider;
        }

        public CreateVmResult CreateVm(TaskV2 task, OperatingSystem os)
        {
            Thread.Sleep(10000);

            if (ConfigurationManager.AppSettings["StatusTask"] == "EndWithError")
            {
                throw new CreateVmException("Don't create VM");
            }

            var result = new CreateVmResult
            {
                MachineGuid = task.GetOptions<CreateVmOptions>().UserVmId,
                IpAddresses = new System.Collections.Generic.List<VmWareRemote.Model.VmWareVirtualMachine.vmIPInfo>()
                {
                    new VmWareRemote.Model.VmWareVirtualMachine.vmIPInfo
                    {
                        IPv4 = "192.168.0.1",
                        IPv6 =  "2001:0:5ef5:79fd:10a6:2543:3f57:fefd",
                        MAC = "E6-F8-9C-41-98-94",
                        NetworkName = "Test Network 1"
                    },
                    new VmWareRemote.Model.VmWareVirtualMachine.vmIPInfo
                    {
                        IPv4 = "192.168.0.2",
                        IPv6 =  "2002:0:5ef5:79fd:10a6:2543:3f57:fefd",
                        MAC = "E6-F8-9C-41-98-95",
                        NetworkName = "Test Network 2"
                    }
                },
                GuestOsAdminPassword = "fake control pass"
            };
            return result;
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

        public Guid BackupVm(TaskV2 taskEntity)
        {
            return Guid.NewGuid();
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
