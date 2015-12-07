using System;
using Crytex.Model.Models;
using Crytex.Model.Exceptions;

namespace Crytex.ExecutorTask.TaskHandler.HyperV
{
    public class HyperVBackupTaskHandler : BaseHyperVTaskHandler, ITaskHandler
    {
        public HyperVBackupTaskHandler(TaskV2 task, IHyperVControl control, string hostName) : base(task, control, hostName) { }

        protected override TaskExecutionResult ExecuteLogic()
        {
            var taskExecutionResult = new BackupTaskExecutionResult();
            try
            {
                var backupId = this._hyperVControl.BackupVm(this.TaskEntity);
                taskExecutionResult.Success = true;
                taskExecutionResult.BackupGuid = backupId;
            }
            catch (Exception ex) when (ex is CreateVmException || ex is InvalidIdentifierException)
            {
                taskExecutionResult.Success = false;
                taskExecutionResult.ErrorMessage = ex.Message;
            }

            return taskExecutionResult;
        }
    }
}
