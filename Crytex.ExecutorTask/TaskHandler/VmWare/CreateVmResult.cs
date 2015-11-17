using System;

namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public class CreateVmResult
    {
        public Guid MachineGuid { get; set; }
        public string GuestOsAdminPassword { get; set; }
    }
}