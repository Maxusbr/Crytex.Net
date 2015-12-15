﻿using Crytex.ExecutorTask.TaskHandler.HyperV;
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
        private IOperatingSystemsService _operatingSystemService;

        public TaskHandlerFactory(IOperatingSystemsService operatingSystemService)
        {
            this._operatingSystemService = operatingSystemService;
            this._hyperVTaskHandlerMappings = new Dictionary<TypeTask, Func<TaskV2, HyperVHost, BaseTaskHandler>>
            {
                {TypeTask.CreateVm, this.GetCreateVmTaskHandler},
                {TypeTask.UpdateVm, this.GetUpdateVmTaskHandler},
                {TypeTask.ChangeStatus, this.GetChangeVmStatusTaskHandler},
                {TypeTask.RemoveVm, this.GetRemoveVmTaskHandler},
                {TypeTask.Backup, this.GetBackupVmTaskHandler }
            };

            this._vmWareTaskHandlerMappings = new Dictionary<TypeTask, Func<TaskV2, VmWareVCenter, BaseTaskHandler>>
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
                this._operatingSystemService, host.Host);

            return handler;
        }

        private BaseTaskHandler GetCreateVmTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            var handler = new VmWareCreateTaskHandler(task, this.CreateVmWareControl(vCenter), this._operatingSystemService,
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

        private BaseTaskHandler GetBackupVmTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            var handler = new VmWareBackupTaskHandler(task, this.CreateVmWareControl(vCenter), vCenter.ServerAddress);

            return handler;
        }

        private BaseTaskHandler GetBackupVmTaskHandler(TaskV2 task, HyperVHost host)
        {
            var handler = new HyperVBackupTaskHandler(task, this.CreateHyperVControl(task, host), host.Host);

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
