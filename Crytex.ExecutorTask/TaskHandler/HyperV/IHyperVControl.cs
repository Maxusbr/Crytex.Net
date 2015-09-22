using Crytex.Model.Models;
using System;
namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public interface IHyperVControl
    {
        Guid CreateVm(CreateVmTask task);

        void UpdateVm(UpdateVmTask updateVmTask);

        void StartVm(string machineName);

        void StopVm(string machineName);

        void RemoveVm(string machineName);
    }
}
