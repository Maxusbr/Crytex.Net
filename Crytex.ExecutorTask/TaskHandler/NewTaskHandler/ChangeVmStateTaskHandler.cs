using Crytex.Model.Models;
using Crytex.Virtualization.Base;
using System;

namespace Crytex.ExecutorTask.TaskHandler
{
    internal class ChangeVmStateTaskHandler : BaseNewTaskHandler
    {
        public ChangeVmStateTaskHandler(TaskV2 task, IProviderVM virtualizationProvider, Guid virtualizationServerEntityId) 
            : base(task, virtualizationProvider, virtualizationServerEntityId) { }

        protected override TaskExecutionResult ExecuteLogic()
        {
            var result = new TaskExecutionResult();
            var options = this.TaskEntity.GetOptions<ChangeStatusOptions>();
            var vmName = options.VmId.ToString();

            try
            {
                this.VirtualizationProvider.ConnectToServer();
                var vm = this.VirtualizationProvider.GetMachinesByName(vmName);

                switch (options.TypeChangeStatus)
                {
                    case TypeChangeStatus.Start:
                        vm.Start(false);
                        break;
                    case TypeChangeStatus.Stop:
                        vm.Stop();
                        break;
                    case TypeChangeStatus.Reload:
                        vm.Reboot();
                        break;
                    default:
                        throw new ApplicationException("This TypeChangeStatus is not supprted yet");
               }
                result.Success = true;
            }
            catch(Exception e)
            {
                result.Success = false;
                result.ErrorMessage = e.Message;
            }

            return result;
        }
    }
}
