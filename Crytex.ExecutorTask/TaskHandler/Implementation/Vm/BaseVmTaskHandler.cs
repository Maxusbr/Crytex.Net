using System;
using Crytex.Model.Models;
using Crytex.Virtualization.Base;

namespace Crytex.ExecutorTask.TaskHandler.Implementation.Vm
{
    abstract class BaseVmTaskHandler : BaseTaskHandler
    {

        protected IProviderVM VirtualizationProvider { get; }
        // VmWareVCenterId or HyperVHostId
        public Guid VirtualizationServerEnitityId { get; }

        protected abstract TaskExecutionResult ExecuteVmLogic();

        public BaseVmTaskHandler(TaskV2 task, IProviderVM virtualizationProvider, Guid virtualizationServerEntityId) 
            : base(task)
        {
            VirtualizationProvider = virtualizationProvider;
            VirtualizationServerEnitityId = virtualizationServerEntityId;
        }

        protected override TaskExecutionResult ExecuteLogic()
            {
            var taskResult = ExecuteVmLogic();

            taskResult.VirtualizationServerEnitityId = this.VirtualizationServerEnitityId;

            return taskResult;
        }

        protected void ConnectProvider()
        {
            var connectiontResult = this.VirtualizationProvider.ConnectToServer();
            if (connectiontResult.IsError)
            {
                throw new ApplicationException(connectiontResult.ErrorMessage);
            }
        }
    }
}