using System;
using Crytex.Model.Models;
using Crytex.Virtualization.Base;

namespace Crytex.ExecutorTask.TaskHandler
{
    internal class DeleteSnapshotTaskHandler : BaseNewTaskHandler
    {
        public DeleteSnapshotTaskHandler(TaskV2 task, IProviderVM virtualizationProvider, Guid virtualizationServerEntityId) 
            : base(task, virtualizationProvider, virtualizationServerEntityId)
        {
        }

        protected override TaskExecutionResult ExecuteLogic()
        {
            Console.WriteLine("Snapshot deleting task");
            var result = new TaskExecutionResult();
            var options = this.TaskEntity.GetOptions<DeleteSnapshotOptions>();
            var vmName = options.VmId.ToString();

            try
            {
                this.VirtualizationProvider.ConnectToServer();
                var vm = this.VirtualizationProvider.GetMachinesByName(vmName);

                var snapshotServerName = options.SnapshotId.ToString();
                var deleteSnapshotResult = vm.DeleteSnapshot(snapshotServerName, options.DeleteWithChildrens);

                if (deleteSnapshotResult.IsError)
                {
                    throw new ApplicationException(deleteSnapshotResult.ErrorMessage);
                }

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
