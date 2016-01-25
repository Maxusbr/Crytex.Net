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

                this.ConnectProvider();

                var vm = this.VirtualizationProvider.GetMachinesByName(updateVmTaskOptions.VmId.ToString());

                vm.NumCPU = updateVmTaskOptions.Cpu;
                vm.Memory = updateVmTaskOptions.Ram;
                vm.VirtualDrives.Drives.First().ResizeDisk(updateVmTaskOptions.HddGB);
                var modifyResult = vm.Modify();
                if (modifyResult.IsError)
                {
                    throw new ApplicationException(modifyResult.ErrorMessage);
                }

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
