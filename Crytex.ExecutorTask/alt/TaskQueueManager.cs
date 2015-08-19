using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Crytex.ExecutorTask.alt
{
    public class TaskQueueManager
    {
        private BufferBlock<ITaskHandler> _taskHandlerBuffer = new BufferBlock<ITaskHandler>();
        private BufferBlock<TaskExecutionResult> _taskResultsBuffer = new BufferBlock<TaskExecutionResult>();
        private TaskHandlerFactory _taskHandlerFactory = new TaskHandlerFactory();

        public void AddToQueue(BaseTask task)
        {
            var handler = this._taskHandlerFactory.GetHandler(task);
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
                IList<ITaskHandler> tasksHandlers;
                if (this._taskHandlerBuffer.TryReceiveAll(out tasksHandlers))
                {
                    foreach (var handler in tasksHandlers)
                    {
                        var result = handler.Execute();
                        this._taskResultsBuffer.Post(result);
                    }
                }
            }
        }
    }
}
