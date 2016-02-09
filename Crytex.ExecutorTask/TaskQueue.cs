using Crytex.ExecutorTask.TaskHandler;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace Crytex.ExecutorTask
{
    public class TaskQueue
    {
        private BufferBlock<ITaskHandler> _taskHandlerBuffer = new BufferBlock<ITaskHandler>();
        private BufferBlock<TaskExecutionResult> _taskResultsBuffer = new BufferBlock<TaskExecutionResult>();
        private bool _taskIsExecuting = false;

        public void AddToQueue(ITaskHandler handler)
        {
            _taskHandlerBuffer.Post(handler);
        }

        public void ExecuteAsync()
        {
            var thread = new System.Threading.Thread(this.ExecuteInner);
            thread.Start();
        }

        public int GetQueueSize()
        {
            var size = this._taskHandlerBuffer.Count;

            if (this._taskIsExecuting)
            {
                size++;
            }

            return size;
        }

        private void ExecuteInner()
        {
            while (true)
            {
                var handler = this._taskHandlerBuffer.ReceiveAsync().Result;

                this._taskIsExecuting = true;
                var result = handler.Execute();
                this._taskIsExecuting = false;

                this._taskResultsBuffer.Post(result);
            }
        }
    }
}
