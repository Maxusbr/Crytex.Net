﻿using System;
using Crytex.Model.Models;
using Crytex.Virtualization.Base;

namespace Crytex.ExecutorTask.TaskHandler.Implementation.Vm
{
    internal class ChangeVmStateTaskHandler : BaseVmTaskHandler
    {
        public ChangeVmStateTaskHandler(TaskV2 task, IProviderVM virtualizationProvider, Guid virtualizationServerEntityId) 
            : base(task, virtualizationProvider, virtualizationServerEntityId) { }

        protected override TaskExecutionResult ExecuteVmLogic()
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
                        vm.Reboot(true);
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
