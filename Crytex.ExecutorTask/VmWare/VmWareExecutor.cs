using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks.Dataflow;
using Crytex.ExecutorTask;
using Project.Model.Models;
using System.Threading.Tasks;

namespace Crytex.ExecutorTask.VmWare
{
    public class VmWareExecutor : BaseTaskExecutor, ITaskExecutor
    {
       public VmWareExecutor(BufferBlock<CreateVmTask> createTasksBuffer, BufferBlock<UpdateVmTask> updateTasksBuffer,
            BufferBlock<StandartVmTask> standartTaskBuffer)
            : base(createTasksBuffer, updateTasksBuffer, standartTaskBuffer) { }

        public override void ProcessCreateTask(CreateVmTask task)
        {
            Console.WriteLine("New create VmWare task with id {0}", task.Id);
        }

        public override void ProcessUpdateTask(UpdateVmTask task)
        {
            Console.WriteLine("New update VmWare task with id {0}", task.Id);
        }

        public override void ProcessStandartTask(StandartVmTask task)
        {
            Console.WriteLine("New standart VmWare task with id {0}", task.Id);
        }
    }
}
