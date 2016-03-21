using Crytex.Model.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using VmWareRemote.Model;

namespace Crytex.ExecutorTask.TaskHandler
{
    public class TaskExecutionResult
    {
        public TaskV2 TaskEntity { get; set; }

        public bool Success { get; set; }

        public string ErrorMessage { get; set; }

        // VmWareVCenterId or HyperVHostId
        public Guid VirtualizationServerEnitityId { get; set; }
    }

    public class CreateVmTaskExecutionResult : TaskExecutionResult
    {
        public Guid MachineGuid { get; set; }
        public string GuestOsPassword { get; internal set; }
        public IEnumerable<VmIpAddress> IpAddresses { get; internal set; }
    }

    public class BackupTaskExecutionResult : TaskExecutionResult
    {
        public Guid BackupGuid { get; set; }
    }

    public class CreateSnapshotExecutionResult : TaskExecutionResult
    {
        public Guid SnapshotGuid { get; set; }
    }

    public class CreateGameServerTaskExecutionResult : TaskExecutionResult
    {
        public string ServerNewPassword { get; set; }
    }
}
