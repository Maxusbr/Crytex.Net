using Crytex.Model.Models;
using Crytex.Virtualization.Base;
using System;

namespace Crytex.ExecutorTask.TaskHandler
{
    internal class BackupVmTaskHandler : BaseNewTaskHandler
    {
        public BackupVmTaskHandler(TaskV2 task, IProviderVM virtualizationProvider, Guid virtualizationServerEntityId) 
            : base(task, virtualizationProvider, virtualizationServerEntityId) { }

        protected override TaskExecutionResult ExecuteLogic()
        {
            var result = new BackupTaskExecutionResult();
            var options = this.TaskEntity.GetOptions<BackupOptions>();
            var vmName = this.TaskEntity.ResourceId.ToString();

            try
            {
                this.VirtualizationProvider.ConnectToServer();
                var vm = this.VirtualizationProvider.GetMachinesByName(vmName);

                var backupServerName = options.VmBackupId.ToString();
                vm.BackupManager.StartBackup(backupServerName);
                result.BackupGuid = options.VmBackupId;

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
