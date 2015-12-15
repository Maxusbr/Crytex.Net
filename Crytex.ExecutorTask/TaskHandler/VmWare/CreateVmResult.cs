using System;
using System.Collections.Generic;
using VmWareRemote.Model;

namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public class CreateVmResult
    {
        public Guid MachineGuid { get; set; }
        public string GuestOsAdminPassword { get; set; }
        public List<VmWareVirtualMachine.vmIPInfo> IpAddresses { get; internal set; }
    }
}