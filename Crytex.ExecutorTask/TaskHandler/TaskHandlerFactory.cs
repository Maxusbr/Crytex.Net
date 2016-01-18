using Crytex.ExecutorTask.TaskHandler.HyperV;
using Crytex.ExecutorTask.TaskHandler.VmWare;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Virtualization.Fake;
using HyperVRemote;
using HyperVRemote.Source.Implementation;
using System;
using System.Collections.Generic;
using VmWareRemote.Implementations;
using VmWareRemote.Interface;
using VmWareRemote.Model;

namespace Crytex.ExecutorTask.TaskHandler
{
    public class TaskHandlerFactory
    {
        private IDictionary<TypeTask, Func<TaskV2, HyperVHost, ITaskHandler>> _hyperVTaskHandlerMappings;
        private IDictionary<TypeTask, Func<TaskV2, VmWareVCenter, ITaskHandler>> _vmWareTaskHandlerMappings;
        private IOperatingSystemsService _operatingSystemService;

        public TaskHandlerFactory(IOperatingSystemsService operatingSystemService)
        {
            this._operatingSystemService = operatingSystemService;
            this._hyperVTaskHandlerMappings = new Dictionary<TypeTask, Func<TaskV2, HyperVHost, ITaskHandler>>
            {
                {TypeTask.CreateVm, this.GetCreateVmTaskHandler},
                {TypeTask.UpdateVm, this.GetUpdateVmTaskHandler},
                {TypeTask.ChangeStatus, this.GetChangeVmStatusTaskHandler},
                {TypeTask.RemoveVm, this.GetRemoveVmTaskHandler},
                {TypeTask.Backup, this.GetBackupVmTaskHandler }
            };

            this._vmWareTaskHandlerMappings = new Dictionary<TypeTask, Func<TaskV2, VmWareVCenter, ITaskHandler>>
            {
                {TypeTask.CreateVm, this.GetCreateVmTaskHandler},
                {TypeTask.UpdateVm, this.GetUpdateVmTaskHandler},
                {TypeTask.ChangeStatus, this.GetChangeVmStatusTaskHandler},
                {TypeTask.RemoveVm, this.GetRemoveVmTaskHandler},
                {TypeTask.Backup, this.GetBackupVmTaskHandler }
            };
        }


        public ITaskHandler GetHyperVHandler(TaskV2 task, HyperVHost hyperVHost)
        {
            var typeTask = task.TypeTask;
            var handler = this._hyperVTaskHandlerMappings[typeTask].Invoke(task, hyperVHost);
            
            return handler;
        }

        public ITaskHandler GetVmWareHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            var typeTask = task.TypeTask;
            var handler = this._vmWareTaskHandlerMappings[typeTask].Invoke(task, vCenter);

            return handler;
        }

        #region Private methods
        private BaseNewTaskHandler GetCreateVmTaskHandler(TaskV2 task, HyperVHost host)
        {
            throw new NotImplementedException();
        }

        private BaseNewTaskHandler GetCreateVmTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            throw new NotImplementedException();
        }

        private BaseNewTaskHandler GetUpdateVmTaskHandler(TaskV2 task, HyperVHost host)
        {
            throw new NotImplementedException();
        }

        private BaseNewTaskHandler GetUpdateVmTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            throw new NotImplementedException();
        }

        private BaseNewTaskHandler GetChangeVmStatusTaskHandler(TaskV2 task, HyperVHost host)
        {
            var provider = new FakeProvider(Virtualization.Base.ProviderVirtualization.Hyper_V);
            var handler = new ChangeVmStateTaskHandler(task, provider, host.Id);

            return handler;
        }

        private BaseNewTaskHandler GetChangeVmStatusTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            var provider = new FakeProvider(Virtualization.Base.ProviderVirtualization.Hyper_V);
            var handler = new ChangeVmStateTaskHandler(task, provider, vCenter.Id);

            return handler;
        }

        private BaseNewTaskHandler GetRemoveVmTaskHandler(TaskV2 task, HyperVHost host)
        {
            throw new NotImplementedException();
        }

        private BaseNewTaskHandler GetRemoveVmTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            throw new NotImplementedException();
        }

        private BaseNewTaskHandler GetBackupVmTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            var provider = new FakeProvider(Virtualization.Base.ProviderVirtualization.Hyper_V);
            var handler = new BackupVmTaskHandler(task, provider, vCenter.Id);

            return handler;
        }

        private BaseNewTaskHandler GetBackupVmTaskHandler(TaskV2 task, HyperVHost host)
        {
            var provider = new FakeProvider(Virtualization.Base.ProviderVirtualization.Hyper_V);
            var handler = new BackupVmTaskHandler(task, provider, host.Id);

            return handler;
        }

        private IVmWareControl CreateVmWareControl(VmWareVCenter vCenter)
        {
      
            var control = new FakeVmWareControl(null);

            return control;
        }

        private IHyperVControl CreateHyperVControl(TaskV2 task, HyperVHost host)
        {
     
            var control = new FakeHyperVControl(null);

            return control;
        }
        #endregion // Private methods
    }
}
