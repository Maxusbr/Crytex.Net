using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Crytex.ExecutorTask.Hyper_V;
using Crytex.ExecutorTask.VmWare;
using Project.Data.IRepository;
using System.Threading.Tasks.Dataflow;
using Project.Model.Models;
using Project.Data.Infrastructure;
using Project.Service.IService;

namespace Crytex.ExecutorTask
{
    public class Executor
    {
        private HyperVExecutor _hyperVExecutor;
        private VmWareExecutor _vmWareExecutor;
        private ITaskVmService _taskVmService;

        private ITargetBlock<CreateVmTask> _hyperVCreateTasks;
        private ITargetBlock<UpdateVmTask> _hyperVUpdateTasks;
        private ITargetBlock<StandartVmTask> _hyperVStandartTasks;

        private ITargetBlock<CreateVmTask> _vmWareCreateTasks;
        private ITargetBlock<UpdateVmTask> _vmWareUpdateTasks;
        private ITargetBlock<StandartVmTask> _vmWareStandartTasks;

        public Executor(ITaskVmService taskVmService)
        {
            this._taskVmService = taskVmService;

            var hyperVCreateTasksBuffer = new BufferBlock<CreateVmTask>();
            var hyperVUpdateTasksBuffer = new BufferBlock<UpdateVmTask>();
            var hyperVStandartTasksBuffer = new BufferBlock<StandartVmTask>();

            this._hyperVCreateTasks = hyperVCreateTasksBuffer;
            this._hyperVUpdateTasks = hyperVUpdateTasksBuffer;
            this._hyperVStandartTasks = hyperVStandartTasksBuffer;

            this._hyperVExecutor = new HyperVExecutor(hyperVCreateTasksBuffer, hyperVUpdateTasksBuffer, hyperVStandartTasksBuffer);

            var vmWareCreateTasksBuffer = new BufferBlock<CreateVmTask>();
            var vmWareUpdateTasksBuffer = new BufferBlock<UpdateVmTask>();
            var vmWareStandartTasksBuffer = new BufferBlock<StandartVmTask>();

            this._vmWareCreateTasks = vmWareCreateTasksBuffer;
            this._vmWareUpdateTasks = vmWareUpdateTasksBuffer;
            this._vmWareStandartTasks = vmWareStandartTasksBuffer;

            this._vmWareExecutor = new VmWareExecutor(vmWareCreateTasksBuffer, vmWareUpdateTasksBuffer, vmWareStandartTasksBuffer);
        }

        public void Run()
        {
            var runnerBackgroundTask = new System.Threading.Thread(this.RunInner);
            runnerBackgroundTask.Name = "MainRun";
            runnerBackgroundTask.Start();

            var vmWareExecutorBackgroundTask = new System.Threading.Thread(this._vmWareExecutor.ExecuteAll);
            vmWareExecutorBackgroundTask.Name = "VmWare executor";
            vmWareExecutorBackgroundTask.Start();

            var hyperVExecutorBackgroundTask = new System.Threading.Thread(this._hyperVExecutor.ExecuteAll);
            hyperVExecutorBackgroundTask.Name = "HyperV executor";
            hyperVExecutorBackgroundTask.Start();

            return;
        }

        private void RunInner()
        {
            while (true)
            {
                var createTasks = this._taskVmService.GetPendingCreateTasks();
                var updateTasks = this._taskVmService.GetPendingUpdateTasks();
                var standartTasks = this._taskVmService.GetPendingStandartTasks();

                this.ProcessTaskList<CreateVmTask>(this._hyperVCreateTasks, this._vmWareCreateTasks, createTasks);
                this.ProcessTaskList<UpdateVmTask>(this._hyperVUpdateTasks, this._vmWareUpdateTasks, updateTasks);
                this.ProcessTaskList<StandartVmTask>(this._hyperVStandartTasks, this._vmWareStandartTasks, standartTasks);

                Thread.Sleep(EXECUTOR_TIMEOUT);
            }
        }

        private void ProcessTaskList<T>(ITargetBlock<T> hyperVTargetBlock, ITargetBlock<T> vmWareTargetBlock,
            IEnumerable<T> tasks) where T : BaseTask
        {
            foreach (var task in tasks)
            {
                this._taskVmService.UpdateTaskStatus<T>(task.Id, StatusTask.Processing);
                switch (task.Virtualization)
                {
                    case TypeVirtualization.HyperV:
                        hyperVTargetBlock.Post(task);
                        break;
                    case TypeVirtualization.WmWare:
                        vmWareTargetBlock.Post(task);
                        break;
                }
            }
        }

        private const int EXECUTOR_TIMEOUT = 1000;
        
    }
}
