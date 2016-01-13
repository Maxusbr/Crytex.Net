using Crytex.Model.Models;
using Crytex.Virtualization.Base;
using System;

namespace Crytex.ExecutorTask.TaskHandler
{
    internal abstract class BaseNewTaskHandler : ITaskHandler
    {
        public TaskV2 TaskEntity { get; protected set; }

        public event EventHandler<TaskExecutionResult> ProcessingFinished;
        public event EventHandler<TaskV2> ProcessingStarted;

        protected abstract TaskExecutionResult ExecuteLogic();
        protected IProviderVM VirtualizationProvider { get; }
        public TypeVirtualization TypeVirtualization { get; }
        // VmWareVCenterId or HyperVHostId
        public Guid VirtualizationServerEnitityId { get; }

        protected BaseNewTaskHandler(TaskV2 task, IProviderVM virtualizationProvider, Guid virtualizationServerEntityId)
        {
            this.TaskEntity = task;
            this.VirtualizationServerEnitityId = virtualizationServerEntityId;
        }

        public TaskExecutionResult Execute()
        {
            if (this.ProcessingStarted != null)
            {
                this.ProcessingStarted.Invoke(this, this.TaskEntity);
            }

            var taskResult = this.ExecuteLogic();
            taskResult.TaskEntity = this.TaskEntity;

            taskResult.TypeVirtualization = this.TypeVirtualization;
            taskResult.VirtualizationServerEnitityId = this.VirtualizationServerEnitityId;

            if (this.ProcessingFinished != null)
            {
                this.ProcessingFinished(this, taskResult);
            }


            return taskResult;
        }
    }
}