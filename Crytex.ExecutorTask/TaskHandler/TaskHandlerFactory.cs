using Crytex.ExecutorTask.TaskHandler.HyperV;
using Crytex.ExecutorTask.TaskHandler.VmWare;
using Crytex.Model.Models;
using Crytex.Service.IService;
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
        private IDictionary<TypeTask, Func<TaskV2, HyperVHost, BaseTaskHandler>> _hyperVTaskHandlerMappings;
        private IDictionary<TypeTask, Func<TaskV2, VmWareVCenter, BaseTaskHandler>> _vmWareTaskHandlerMappings;
        private IServerTemplateService _serverTemplateService;

        public TaskHandlerFactory(IServerTemplateService serverTemplateService)
        {
            this._serverTemplateService = serverTemplateService;
            this._hyperVTaskHandlerMappings = new Dictionary<TypeTask, Func<TaskV2, HyperVHost, BaseTaskHandler>>
            {
                {TypeTask.CreateVm, this.GetCreateVmTaskHandler},
                {TypeTask.UpdateVm, this.GetUpdateVmTaskHandler},
                {TypeTask.ChangeStatus, this.GetChangeVmStatusTaskHandler},
                {TypeTask.RemoveVm, this.GetRemoveVmTaskHandler}
            };

            this._vmWareTaskHandlerMappings = new Dictionary<TypeTask, Func<TaskV2, VmWareVCenter, BaseTaskHandler>>
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
            handler.TypeVirtualization = TypeVirtualization.HyperV;
            handler.VirtualizationServerEnitityId = hyperVHost.Id;
            
            return handler;
        }

        public ITaskHandler GetVmWareHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            var typeTask = task.TypeTask;
            var handler = this._vmWareTaskHandlerMappings[typeTask].Invoke(task, vCenter);
            handler.TypeVirtualization = TypeVirtualization.VmWare;
            handler.VirtualizationServerEnitityId = vCenter.Id;

            return handler;
        }

        #region Private methods
        private BaseTaskHandler GetCreateVmTaskHandler(TaskV2 task, HyperVHost host)
        {
            var handler = new HyperVCreateVmTaskHandler(task, this.CreateHyperVControl(task, host), 
                this._serverTemplateService, host.Host);

            return handler;
        }

        private BaseTaskHandler GetCreateVmTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            var handler = new VmWareCreateTaskHandler(task, this.CreateVmWareControl(vCenter), this._serverTemplateService,
                vCenter.ServerAddress);

            return handler;
        }

        private BaseTaskHandler GetUpdateVmTaskHandler(TaskV2 task, HyperVHost host)
        {
            var handler = new HyperVUpdateVmTaskHandler(task, this.CreateHyperVControl(task, host), host.Host);
            
            return handler;
        }

        private BaseTaskHandler GetUpdateVmTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            var handler = new VmWareUpdateTaskHandler(task, this.CreateVmWareControl(vCenter), vCenter.ServerAddress);

            return handler;
        }

        private BaseTaskHandler GetChangeVmStatusTaskHandler(TaskV2 task, HyperVHost host)
        {
            var handler = new HyperVStandartTaskHandler(task, this.CreateHyperVControl(task, host), host.Host);

            return handler;
        }

        private BaseTaskHandler GetChangeVmStatusTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            var handler = new VmWareStandartVmWareTaskHandler(task, this.CreateVmWareControl(vCenter), vCenter.ServerAddress);

            return handler;
        }

        private BaseTaskHandler GetRemoveVmTaskHandler(TaskV2 task, HyperVHost host)
        {
            throw new NotImplementedException();
        }

        private BaseTaskHandler GetRemoveVmTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            var handler = new VmWareRemoveVmTaskHandler(task, this.CreateVmWareControl(vCenter), vCenter.ServerAddress);

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
            var control = new FakeHyperVControl(hyperVProvider);

            return control;
        }
        #endregion // Private methods
    }
}
