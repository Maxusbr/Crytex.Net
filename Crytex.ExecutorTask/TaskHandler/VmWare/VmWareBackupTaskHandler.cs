using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public class VmWareBackupTaskHandler : BaseVmWareTaskHandler, ITaskHandler
    {
        public VmWareBackupTaskHandler(TaskV2 task, IVmWareControl control, string hostName) : base(task, control, hostName)
        {
        }

        protected override TaskExecutionResult ExecuteLogic()
        {
            var taskExecutionResult = new BackupTaskExecutionResult();
            try
            {
                var backupId = this._vmWareControl.BackupVm(this.TaskEntity);
                taskExecutionResult.Success = true;
                taskExecutionResult.BackupGuid = backupId;
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
