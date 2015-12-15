using Crytex.Model.Models;
using System;
using OperatingSystem = Crytex.Model.Models.OperatingSystem;

namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public interface IHyperVControl
    {
        CreateVmResult CreateVm(TaskV2 task, OperatingSystem os);

        void UpdateVm(TaskV2 updateVmTask);

        void StartVm(string machineName);

        void StopVm(string machineName);

        void RemoveVm(string machineName);
        Guid BackupVm(TaskV2 taskEntity);
    }
}
