using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Model.Models;
using System.Threading.Tasks.Dataflow;

namespace Crytex.ExecutorTask.Hyper_V
{
    public class HyperVExecutor : BaseTaskExecutor, ITaskExecutor
    {
        public HyperVExecutor(BufferBlock<CreateVmTask> createTasksBuffer, BufferBlock<UpdateVmTask> updateTasksBuffer,
            BufferBlock<StandartVmTask> standartTaskBuffer)
            : base(createTasksBuffer, updateTasksBuffer, standartTaskBuffer) { }

        public override void ProcessCreateTask(CreateVmTask task)
        {
            Console.WriteLine("New create HyperV task with id {0}", task.Id);
        }

        public override void ProcessUpdateTask(UpdateVmTask task)
        {
            Console.WriteLine("New update HyperV task with id {0}", task.Id);
        }

        public override void ProcessStandartTask(StandartVmTask task)
        {
            Console.WriteLine("New standart HyperV task with id {0}", task.Id);
        }
    }
}
