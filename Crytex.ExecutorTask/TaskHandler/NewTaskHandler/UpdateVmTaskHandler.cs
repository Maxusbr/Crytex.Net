using System;
using Crytex.Model.Models;
using Crytex.Virtualization.Base;
using System.Linq;

namespace Crytex.ExecutorTask.TaskHandler
{
    internal class UpdateVmTaskHandler : BaseNewTaskHandler
    {
        public UpdateVmTaskHandler(TaskV2 task, IProviderVM virtualizationProvider, Guid virtualizationServerEntityId) 
            : base(task, virtualizationProvider, virtualizationServerEntityId)
        {
        }

        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine("Update vm task VmWare");
            var taskExecutionResult = new CreateVmTaskExecutionResult();
            try
            {
                var updateVmTaskOptions = this.TaskEntity.GetOptions<UpdateVmOptions>();

                this.VirtualizationProvider.ConnectToServer();

                var vm = this.VirtualizationProvider.GetMachinesByName(updateVmTaskOptions.VmId.ToString());

                vm.NumCPU = updateVmTaskOptions.Cpu;
                vm.Memory = updateVmTaskOptions.Ram;
                vm.VirtualDrives.Drives.First().ResizeDisk(updateVmTaskOptions.Hdd);
                vm.Modify();

                taskExecutionResult.Success = true;
            }
            catch (Exception ex)
            {
                taskExecutionResult.Success = false;
                taskExecutionResult.ErrorMessage = ex.Message;
            }

            return taskExecutionResult;
        }
    }
}
