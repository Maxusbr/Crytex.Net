using Project.Model.Models;
using Project.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Crytex.ExecutorTask.alt
{
    public class TaskManager
    {
        private ITaskVmService _taskService;
        private TaskQueueManager _vmWareTaskQueueExecutor = new TaskQueueManager();
        private TaskQueueManager _hyperVTaskQueueExecutor = new TaskQueueManager();

        public TaskManager(ITaskVmService taskService)
        {
            this._taskService = taskService;
        }

        public void Run()
        {
            var thread = new System.Threading.Thread(this.RunInner);
            thread.Start();

            this._hyperVTaskQueueExecutor.ExecuteAsync();
            this._vmWareTaskQueueExecutor.ExecuteAsync();
        }

        private void RunInner()
        {
            while (true)
            {
                var createTasks = this._taskService.GetPendingCreateTasks();
                var updateTasks = this._taskService.GetPendingUpdateTasks();
                var standartTasks = this._taskService.GetPendingStandartTasks();

                this.ProcessTaskList<CreateVmTask>(createTasks);
                this.ProcessTaskList<UpdateVmTask>(updateTasks);
                this.ProcessTaskList<StandartVmTask>(standartTasks);

                Thread.Sleep(EXECUTOR_TIMEOUT);
            }
        }

        private void ProcessTaskList<T>(IEnumerable<T> tasks) where T : BaseTask
        {
            foreach (var task in tasks)
            {
                this._taskService.UpdateTaskStatus<T>(task.Id, StatusTask.Processing);
                switch (task.Virtualization)
                {
                    case TypeVirtualization.HyperV:
                        this._hyperVTaskQueueExecutor.AddToQueue(task);
                        break;
                    case TypeVirtualization.WmWare:
                        this._vmWareTaskQueueExecutor.AddToQueue(task);
                        break;
                }
            }
        }

        private const int EXECUTOR_TIMEOUT = 1000;
    }
}
