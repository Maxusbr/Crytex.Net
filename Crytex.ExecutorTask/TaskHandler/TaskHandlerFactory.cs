using Crytex.ExecutorTask.TaskHandler.HyperV;
using Crytex.ExecutorTask.TaskHandler.VmWare;
using Crytex.Model.Models;
using HyperVRemote;
using HyperVRemote.Source.Implementation;
using System;
using System.Collections.Generic;
using VmWareRemote.Implementations;

namespace Crytex.ExecutorTask.TaskHandler
{
    public class TaskHandlerFactory
    {
        private IDictionary<TypeTask, Func<TaskV2, HyperVHost, ITaskHandler>> _hyperVTaskHandlerMappings;
        private IDictionary<TypeTask, Func<TaskV2, VmWareVCenter, ITaskHandler>> _vmWareTaskHandlerMappings;

        public TaskHandlerFactory()
        {
            this._hyperVTaskHandlerMappings = new Dictionary<TypeTask, Func<TaskV2, HyperVHost, ITaskHandler>>
            {
                {TypeTask.CreateVm, this.GetCreateVmTaskHandler},
                {TypeTask.UpdateVm, this.GetUpdateVmTaskHandler},
                {TypeTask.ChangeStatus, this.GetChangeVmStatusTaskHandler},
                {TypeTask.RemoveVm, this.GetRemoveVmTaskHandler}
            };

            this._vmWareTaskHandlerMappings = new Dictionary<TypeTask, Func<TaskV2, VmWareVCenter, ITaskHandler>>
            {
                {TypeTask.CreateVm, this.GetCreateVmTaskHandler},
                {TypeTask.UpdateVm, this.GetUpdateVmTaskHandler},
                {TypeTask.ChangeStatus, this.GetChangeVmStatusTaskHandler},
                {TypeTask.RemoveVm, this.GetRemoveVmTaskHandler}
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
        private ITaskHandler GetCreateVmTaskHandler(TaskV2 task, HyperVHost host)
        {
            ITaskHandler handler = null;
            handler = new HyperVCreateVmTaskHandler(task, this.CreateHyperVControl(task, host), host.Host);

            return handler;
        }

        private ITaskHandler GetCreateVmTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            ITaskHandler handler = null;
            handler = new VmWareCreateTaskHandler(task, this.CreateVmWareControl(vCenter), vCenter.ServerAddress);

            return handler;
        }

        private ITaskHandler GetUpdateVmTaskHandler(TaskV2 task, HyperVHost host)
        {
            ITaskHandler handler = null;
            handler = new HyperVUpdateVmTaskHandler(task, this.CreateHyperVControl(task, host), host.Host);
            
            return handler;
        }

        private ITaskHandler GetUpdateVmTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            ITaskHandler handler = null;
            handler = new VmWareUpdateTaskHandler(task, this.CreateVmWareControl(vCenter), vCenter.ServerAddress);

            return handler;
        }

        private ITaskHandler GetChangeVmStatusTaskHandler(TaskV2 task, HyperVHost host)
        {
            ITaskHandler handler = null;
            handler = new HyperVStandartTaskHandler(task, this.CreateHyperVControl(task, host), host.Host);

            return handler;
        }

        private ITaskHandler GetChangeVmStatusTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            ITaskHandler handler = null;
            handler = new VmWareStandartVmWareTaskHandler(task, this.CreateVmWareControl(vCenter), vCenter.ServerAddress);

            return handler;
        }

        private ITaskHandler GetRemoveVmTaskHandler(TaskV2 task, HyperVHost host)
        {
            throw new NotImplementedException();
        }

        private ITaskHandler GetRemoveVmTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            ITaskHandler handler = null;
            handler = new VmWareRemoveVmTaskHandler(task, this.CreateVmWareControl(vCenter), vCenter.ServerAddress);

            return handler;
        }

        private IVmWareControl CreateVmWareControl(VmWareVCenter vCenter)
        {
            var vmWareProvider = new VmWareProvider(vCenter.UserName, vCenter.Password, vCenter.ServerAddress);
            var control = new VmWareControl(vmWareProvider);

            return control;
        }

        private IHyperVControl CreateHyperVControl(TaskV2 task, HyperVHost host)
        {
            var configuration = new HyperVConfiguration(host.UserName, host.Password, host.Host);
            var hyperVProvider = new HyperVProvider(configuration);
            var control = new HyperVControl(hyperVProvider);

            return control;
        }
        #endregion // Private methods
    }
}
