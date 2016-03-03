using Crytex.ExecutorTask.Config;
using Crytex.ExecutorTask.TaskHandler.HyperV;
using Crytex.ExecutorTask.TaskHandler.VmWare;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Virtualization._VMware;
using Crytex.Virtualization.Base;
using Crytex.Virtualization.Fake;
using Crytex.Virtualization.HyperV;
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
        private readonly ISnapshotVmService _snapshotVmService;
        private bool _useFakeProviders = false;

        public TaskHandlerFactory(IOperatingSystemsService operatingSystemService, ISnapshotVmService snapshotVmService)
        {
            this._operatingSystemService = operatingSystemService;
            this._snapshotVmService = snapshotVmService;
            this._hyperVTaskHandlerMappings = new Dictionary<TypeTask, Func<TaskV2, HyperVHost, ITaskHandler>>
            {
                {TypeTask.CreateVm, this.GetCreateVmTaskHandler},
                {TypeTask.UpdateVm, this.GetUpdateVmTaskHandler},
                {TypeTask.ChangeStatus, this.GetChangeVmStatusTaskHandler},
                {TypeTask.RemoveVm, this.GetRemoveVmTaskHandler},
                {TypeTask.Backup, this.GetBackupVmTaskHandler },
                {TypeTask.DeleteBackup, this.GetDeleteBackupTaskHandler },
                {TypeTask.CreateSnapshot, this.GetCreateSnapshotTaskHandler },
                {TypeTask.DeleteSnapshot, this.GetDeleteSnapshotTaskHandler },
                {TypeTask.LoadSnapshot, this.GetLoadSnapshotTaskHandler }
            };

            this._vmWareTaskHandlerMappings = new Dictionary<TypeTask, Func<TaskV2, VmWareVCenter, ITaskHandler>>
            {
                {TypeTask.CreateVm, this.GetCreateVmTaskHandler},
                {TypeTask.UpdateVm, this.GetUpdateVmTaskHandler},
                {TypeTask.ChangeStatus, this.GetChangeVmStatusTaskHandler},
                {TypeTask.RemoveVm, this.GetRemoveVmTaskHandler},
                {TypeTask.Backup, this.GetBackupVmTaskHandler },
                {TypeTask.DeleteBackup, this.GetDeleteBackupTaskHandler },
                {TypeTask.CreateSnapshot, this.GetCreateSnapshotTaskHandler },
                {TypeTask.DeleteSnapshot, this.GetDeleteSnapshotTaskHandler },
                {TypeTask.LoadSnapshot, this.GetLoadSnapshotTaskHandler }
            };

            var config = new ExecutorTaskConfig();
            this._useFakeProviders = config.GetUseFakeProviders();
        }

        public ITaskHandler GetHyperVHandler(TaskV2 task, HyperVHost hyperVHost)
        {
            var typeTask = task.TypeTask;
            var handler = this._hyperVTaskHandlerMappings[typeTask].Invoke(task, hyperVHost);
            //var handler = new TestTaskHandler(task);
            
            return handler;
        }

        public ITaskHandler GetVmWareHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            var typeTask = task.TypeTask;
            var handler = this._vmWareTaskHandlerMappings[typeTask].Invoke(task, vCenter);
            //var handler = new TestTaskHandler(task);

            return handler;
        }

        #region Private methods
        private IProviderVM GetProvider(HyperVHost host)
        {
            IProviderVM provider;
            if (!this._useFakeProviders)
            {
                AutorizationInfo userData = new AutorizationInfo
                {
                    ServerAddress = host.Host,
                    UserName = host.UserName,
                    UserPassword = host.Password
                };
                provider = new ProviderHyper_V(userData);
            }
            else
            {
                provider = new FakeProvider(ProviderVirtualization.Hyper_V);
            }

            return provider;
        }
        private IProviderVM GetProvider(VmWareVCenter vCenter)
        {
            IProviderVM provider;
            if (!this._useFakeProviders)
            {
                AutorizationInfo userData = new AutorizationInfo
                {
                    ServerAddress = vCenter.ServerAddress,
                    UserName = vCenter.UserName,
                    UserPassword = vCenter.Password
                };
                provider = new ProviderWMware(userData);
            }
            else
            {
                provider = new FakeProvider(ProviderVirtualization.WMware);
            }

            return provider;
        }
        private BaseNewTaskHandler GetCreateVmTaskHandler(TaskV2 task, HyperVHost host)
        {
            var provider = this.GetProvider(host);
            var handler = new CreateVmTaskHandler(this._operatingSystemService, task, provider, host.Id, host.DefaultVmNetworkName);

            return handler;
        }

        private BaseNewTaskHandler GetCreateVmTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            var provider = this.GetProvider(vCenter);
            var handler = new CreateVmTaskHandler(this._operatingSystemService, task, provider, vCenter.Id, vCenter.DefaultVmNetworkName);

            return handler;
        }

        private BaseNewTaskHandler GetUpdateVmTaskHandler(TaskV2 task, HyperVHost host)
        {
            var provider = this.GetProvider(host);
            var handler = new UpdateVmTaskHandler(task, provider, host.Id);

            return handler;
        }

        private BaseNewTaskHandler GetUpdateVmTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            var provider = this.GetProvider(vCenter);
            var handler = new UpdateVmTaskHandler(task, provider, vCenter.Id);

            return handler;
        }

        private BaseNewTaskHandler GetChangeVmStatusTaskHandler(TaskV2 task, HyperVHost host)
        {
            var provider = this.GetProvider(host);
            var handler = new ChangeVmStateTaskHandler(task, provider, host.Id);

            return handler;
        }

        private BaseNewTaskHandler GetChangeVmStatusTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            var provider = this.GetProvider(vCenter);
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
            var provider = this.GetProvider(vCenter);
            var handler = new BackupVmTaskHandler(task, provider, vCenter.Id);

            return handler;
        }

        private BaseNewTaskHandler GetBackupVmTaskHandler(TaskV2 task, HyperVHost host)
        {
            var provider = this.GetProvider(host);
            var handler = new BackupVmTaskHandler(task, provider, host.Id);

            return handler;
        }


        private ITaskHandler GetDeleteBackupTaskHandler(TaskV2 task, HyperVHost host)
        {
            var provider = this.GetProvider(host);
            var handler = new DeleteVmBackupTaskHandler(task, provider, host.Id);

            return handler;
        }

        private ITaskHandler GetDeleteBackupTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            var provider = this.GetProvider(vCenter);
            var handler = new DeleteVmBackupTaskHandler(task, provider, vCenter.Id);

            return handler;
        }
        private ITaskHandler GetCreateSnapshotTaskHandler(TaskV2 task, HyperVHost host)
        {
            var provider = this.GetProvider(host);
            var handler = new CreateSnapshotTaskHandler(task, provider, host.Id);

            return handler;
        }

        private ITaskHandler GetCreateSnapshotTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            var provider = this.GetProvider(vCenter);
            var handler = new CreateSnapshotTaskHandler(task, provider, vCenter.Id);

            return handler;
        }
        private ITaskHandler GetDeleteSnapshotTaskHandler(TaskV2 task, HyperVHost host)
        {
            var provider = this.GetProvider(host);
            var handler = new DeleteSnapshotTaskHandler(task, provider, host.Id);

            return handler;
        }

        private ITaskHandler GetDeleteSnapshotTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            var provider = this.GetProvider(vCenter);
            var handler = new DeleteSnapshotTaskHandler(task, provider, vCenter.Id);

            return handler;
        }

        private ITaskHandler GetLoadSnapshotTaskHandler(TaskV2 task, HyperVHost host)
        {
            var provider = this.GetProvider(host);
            var handler = new LoadSnapshotTaskHandler(task, provider, host.Id, this._snapshotVmService);

            return handler;
        }

        private ITaskHandler GetLoadSnapshotTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            var provider = this.GetProvider(vCenter);
            var handler = new LoadSnapshotTaskHandler(task, provider, vCenter.Id, this._snapshotVmService);

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
