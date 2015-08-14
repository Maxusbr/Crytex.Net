using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Crytex.ExecutorTask
{
    public abstract class BaseTaskExecutor
    {
        private BufferBlock<StandartVmTask> _standartTaskBuffer;
        private BufferBlock<UpdateVmTask> _updateTasksBuffer;
        private BufferBlock<CreateVmTask> _createTasksBuffer;

        public BaseTaskExecutor(BufferBlock<CreateVmTask> createTasksBuffer, BufferBlock<UpdateVmTask> updateTasksBuffer,
            BufferBlock<StandartVmTask> standartTaskBuffer)
        {
            this._createTasksBuffer = createTasksBuffer;
            this._updateTasksBuffer = updateTasksBuffer;
            this._standartTaskBuffer = standartTaskBuffer;
        }

        public abstract void ProcessCreateTask(CreateVmTask task);
        public abstract void ProcessUpdateTask(UpdateVmTask task);
        public abstract void ProcessStandartTask(StandartVmTask task);

        public void ExecuteAll()
        {
            while (true)
            {
                this.CheckBufferAndExecute(this._createTasksBuffer, this.ProcessCreateTask);
                this.CheckBufferAndExecute(this._updateTasksBuffer, this.ProcessUpdateTask);
                this.CheckBufferAndExecute(this._standartTaskBuffer, this.ProcessStandartTask);
            }
        }

        private void CheckBufferAndExecute<T>(BufferBlock<T> buffer,  Action<T> taskHandler) where T : BaseTask
        {
            IList<T> tasks;
            if (buffer.TryReceiveAll(out tasks))
            {
                foreach (var newTask in tasks)
                {
                    taskHandler(newTask);
                }
            }
        }
    }
}
