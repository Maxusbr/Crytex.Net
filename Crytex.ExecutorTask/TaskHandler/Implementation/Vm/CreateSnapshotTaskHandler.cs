﻿using System;
using Crytex.Model.Models;
using Crytex.Virtualization.Base;

namespace Crytex.ExecutorTask.TaskHandler.Implementation.Vm
{
    internal class CreateSnapshotTaskHandler : BaseVmTaskHandler
    {
        public CreateSnapshotTaskHandler(TaskV2 task, IProviderVM virtualizationProvider, Guid virtualizationServerEntityId) 
            : base(task, virtualizationProvider, virtualizationServerEntityId)
        {
        }

        protected override TaskExecutionResult ExecuteVmLogic()
        {
            Console.WriteLine("Snapshot creating task");
            var result = new CreateSnapshotExecutionResult();
            var options = this.TaskEntity.GetOptions<CreateSnapshotOptions>();
            var vmName = options.VmId.ToString();

            try
            {
                this.VirtualizationProvider.ConnectToServer();
                var vm = this.VirtualizationProvider.GetMachinesByName(vmName);

                var snapshotServerName = options.SnapshotId.ToString();
                var createSnapshotResult = vm.SnapshotManager.CreateSnapshot(snapshotServerName);

                if (createSnapshotResult.IsError)
                {
                    throw new ApplicationException(createSnapshotResult.ErrorMessage);
                }

                result.SnapshotGuid = options.SnapshotId;
                result.Success = true;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.ErrorMessage = e.Message;
            }

            return result;
        }
    }
}
