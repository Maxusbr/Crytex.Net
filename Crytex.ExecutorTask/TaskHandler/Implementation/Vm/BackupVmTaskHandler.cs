﻿using System;
using Crytex.Model.Models;
using Crytex.Virtualization.Base;

namespace Crytex.ExecutorTask.TaskHandler.Implementation.Vm
{
    internal class BackupVmTaskHandler : BaseVmTaskHandler
    {
        public BackupVmTaskHandler(TaskV2 task, IProviderVM virtualizationProvider, Guid virtualizationServerEntityId) 
            : base(task, virtualizationProvider, virtualizationServerEntityId) { }

        protected override TaskExecutionResult ExecuteVmLogic()
        {
            var result = new BackupTaskExecutionResult();
            var options = this.TaskEntity.GetOptions<BackupOptions>();
            var vmName = this.TaskEntity.ResourceId.ToString();

            try
            {
                this.VirtualizationProvider.ConnectToServer();
                var vm = this.VirtualizationProvider.GetMachinesByName(vmName);

                var backupServerName = options.VmBackupId.ToString();
                throw new NotImplementedException();
                // TODO : Refactor this!
                //vm.BackupManager.StartBackup(backupServerName);
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
