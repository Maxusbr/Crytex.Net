using Crytex.Model.Models;

namespace Crytex.ExecutorTask.TaskHandler
{
    public class TaskExecutionResult
    {
        public BaseTask TaskEntity { get; set; }

        public bool Success { get; set; }
    }
}
