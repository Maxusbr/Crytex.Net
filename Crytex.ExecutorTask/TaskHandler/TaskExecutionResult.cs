using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler
{
    public class TaskExecutionResult
    {
        public TaskV2 TaskEntity { get; set; }

        public bool Success { get; set; }

        public string ErrorMessage { get; set; }

        public TypeVirtualization TypeVirtualization { get; set; }
        // VmWareVCenterId or HyperVHostId
        public Guid VirtualizationServerEnitityId { get; set; }
    }

    public class CreateVmTaskExecutionResult : TaskExecutionResult
    {
        public Guid MachineGuid { get; set; }
        public string GuestOsPassword { get; internal set; }
    }

    public class BackupTaskExecutionResult : TaskExecutionResult
    {
        public Guid BackupGuid { get; set; }
    }
}
