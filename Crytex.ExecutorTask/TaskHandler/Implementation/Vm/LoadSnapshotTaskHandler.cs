using System;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Virtualization.Base;

namespace Crytex.ExecutorTask.TaskHandler.Implementation.Vm
{
    internal class LoadSnapshotTaskHandler : BaseVmTaskHandler
    {
        private readonly ISnapshotVmService _snapshotVmService;

        public LoadSnapshotTaskHandler(TaskV2 task, IProviderVM virtualizationProvider, Guid virtualizationServerEntityId,
            ISnapshotVmService snapshotVmService) 
            : base(task, virtualizationProvider, virtualizationServerEntityId)
        {
            this._snapshotVmService = snapshotVmService;
        }

        protected override TaskExecutionResult ExecuteVmLogic()
        {
            Console.WriteLine("Snapshot loading task");
            var result = new TaskExecutionResult();
            var options = this.TaskEntity.GetOptions<LoadSnapshotOptions>();
            var vmName = options.VmId.ToString();

            try
            {
                var snapshot = this._snapshotVmService.GetById(options.SnapshotId);
                if(snapshot.Status != SnapshotStatus.Active)
                {
                    throw new ApplicationException($"Cannot load snapshot with status {snapshot.Status}");
                }

                this.VirtualizationProvider.ConnectToServer();
                var vm = this.VirtualizationProvider.GetMachinesByName(vmName);

                var snapshotServerName = options.SnapshotId.ToString();
                var createSnapshotResult = vm.SnapshotManager.LoadSnapshot(snapshotServerName);

                if (createSnapshotResult.IsError)
                {
                    throw new ApplicationException(createSnapshotResult.ErrorMessage);
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
