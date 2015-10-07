using Crytex.Model.Models;
using System;
namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public interface IHyperVControl
    {
        Guid CreateVm(TaskV2 task);

        void UpdateVm(TaskV2 updateVmTask);

        void StartVm(string machineName);

        void StopVm(string machineName);

        void RemoveVm(string machineName);
    }
}
