using Crytex.Model.Models;
using System;

namespace Crytex.ExecutorTask.TaskHandler
{
    public class TaskExecutionResult
    {
        public BaseTask TaskEntity { get; set; }

        public bool Success { get; set; }

        public Guid MachineGuid { get; set; }

        public string ErrorMessage { get; set; }
    }
}
