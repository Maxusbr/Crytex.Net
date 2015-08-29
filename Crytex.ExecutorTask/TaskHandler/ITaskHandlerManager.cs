namespace Crytex.ExecutorTask.TaskHandler
{
    public interface ITaskHandlerManager
    {
        PendingTaskHandlerBox GetTaskHandlers();
    }
}
