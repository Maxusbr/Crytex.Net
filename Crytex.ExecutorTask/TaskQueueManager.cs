using Crytex.ExecutorTask.TaskHandler;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace Crytex.ExecutorTask
{
    public class TaskQueueManager
    {
        private BufferBlock<ITaskHandler> _taskHandlerBuffer = new BufferBlock<ITaskHandler>();
        private BufferBlock<TaskExecutionResult> _taskResultsBuffer = new BufferBlock<TaskExecutionResult>();

        public void AddToQueue(ITaskHandler handler)
        {
            _taskHandlerBuffer.Post(handler);
        }

        public void ExecuteAsync()
        {
            var thread = new System.Threading.Thread(this.ExecuteInner);
            thread.Start();
        }

        private void ExecuteInner()
        {
            while (true)
            {

                var handler = this._taskHandlerBuffer.ReceiveAsync().Result;
                var result = handler.Execute();
                this._taskResultsBuffer.Post(result);
           
            }
        }
    }
}
