using Crytex.ExecutorTask.Config;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Virtualization.Base;
using Crytex.Virtualization.Fake;
using Crytex.Virtualization.HyperV;
using System;
using System.Collections.Generic;
using Crytex.ExecutorTask.TaskHandler.Implementation.Game;
using Crytex.ExecutorTask.TaskHandler.Implementation.Vm;
using Crytex.GameServers.Fabric;
using Crytex.GameServers.Interface;
using Crytex.GameServers.Models;
using Crytex.Model.Enums;
using Crytex.Model.Models.GameServers;
using VmWareRemote.Implementations;
using VmWareRemote.Interface;
using VmWareRemote.Model;
using Crytex.Virtualization._VMware;

namespace Crytex.ExecutorTask.TaskHandler
{
    public class TaskHandlerFactory
    {
        private IDictionary<TypeTask, Func<TaskV2, HyperVHost, ITaskHandler>> _hyperVTaskHandlerMappings;
        private IDictionary<TypeTask, Func<TaskV2, VmWareVCenter, ITaskHandler>> _vmWareTaskHandlerMappings;
        private IOperatingSystemsService _operatingSystemService;
        private readonly ISnapshotVmService _snapshotVmService;
        private bool _useFakeProviders = false;
        private IDictionary<TypeTask, Func<TaskV2, GameServer, ITaskHandler>> _gameTaskHandlerMappings;

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

            _gameTaskHandlerMappings = new Dictionary<TypeTask, Func<TaskV2, GameServer, ITaskHandler>>
            {
                {TypeTask.CreateGameServer, GetCreateGameServerTaskHandler},
                {TypeTask.DeleteGameServer, GetDeleteGameServerTaskHandler},
                {TypeTask.GameServerChangeStatus, GetChangeStatusGameServerTaskHandler},
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

        public ITaskHandler GetGameTaskHandler(TaskV2 task, GameServer gameServer)
        {
            var typeTask = task.TypeTask;
            var handler = _gameTaskHandlerMappings[typeTask].Invoke(task, gameServer);

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
            //throw new NotImplementedException();
            IProviderVM provider = null;
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
        private BaseTaskHandler GetCreateVmTaskHandler(TaskV2 task, HyperVHost host)
        {
            var provider = this.GetProvider(host);
            var handler = new CreateVmTaskHandler(this._operatingSystemService, task, provider, host.Id, host.DefaultVmNetworkName);

            return handler;
        }

        private BaseTaskHandler GetCreateVmTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            var provider = this.GetProvider(vCenter);
            var handler = new CreateVmTaskHandler(this._operatingSystemService, task, provider, vCenter.Id, vCenter.DefaultVmNetworkName);

            return handler;
        }

        private BaseTaskHandler GetUpdateVmTaskHandler(TaskV2 task, HyperVHost host)
        {
            var provider = this.GetProvider(host);
            var handler = new UpdateVmTaskHandler(task, provider, host.Id);

            return handler;
        }

        private BaseTaskHandler GetUpdateVmTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            var provider = this.GetProvider(vCenter);
            var handler = new UpdateVmTaskHandler(task, provider, vCenter.Id);

            return handler;
        }

        private BaseTaskHandler GetChangeVmStatusTaskHandler(TaskV2 task, HyperVHost host)
        {
            var provider = this.GetProvider(host);
            var handler = new ChangeVmStateTaskHandler(task, provider, host.Id);

            return handler;
        }

        private BaseTaskHandler GetChangeVmStatusTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            var provider = this.GetProvider(vCenter);
            var handler = new ChangeVmStateTaskHandler(task, provider, vCenter.Id);

            return handler;
        }

        private BaseTaskHandler GetRemoveVmTaskHandler(TaskV2 task, HyperVHost host)
        {
            throw new NotImplementedException();
        }

        private BaseTaskHandler GetRemoveVmTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            throw new NotImplementedException();
        }

        private BaseTaskHandler GetBackupVmTaskHandler(TaskV2 task, VmWareVCenter vCenter)
        {
            var provider = this.GetProvider(vCenter);
            var handler = new BackupVmTaskHandler(task, provider, vCenter.Id);

            return handler;
        }

        private BaseTaskHandler GetBackupVmTaskHandler(TaskV2 task, HyperVHost host)
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

        private ITaskHandler GetCreateGameServerTaskHandler(TaskV2 task, GameServer gameServer)
        {
            ConnectParam param = GetGameHostConnectParam(gameServer);
            IGameHost provider = GameServerFactory.Instance.Get(param);
            var handler = new CreateGameServerTaskHandler(task, provider);

            return handler;
        }

        private string MapGameName(GameFamily family)
        {
            string gameName = null;

            switch (family)
            {
                case GameFamily.Cs:
                    gameName = "cs";
                    break;
                case GameFamily.Ark:
                    gameName = "ark";
                    break;
                case GameFamily.Arma3:
                    gameName = "arma3";
                    break;
                case GameFamily.Css:
                    gameName = "css";
                    break;
                case GameFamily.CsGo:
                    gameName = "csgo";
                    break;
                case GameFamily.Cure:
                    gameName = "cure";
                    break;
                case GameFamily.Dods:
                    gameName = "dods";
                    break;
                case GameFamily.GMod:
                    gameName = "gmod";
                    break;
                case GameFamily.L4D:
                    gameName = "l4d";
                    break;
                case GameFamily.L4D2:
                    gameName = "l4d2";
                    break;
                case GameFamily.Minecraft:
                    break;
                case GameFamily.SaMp:
                    break;
                case GameFamily.T2F:
                    gameName = "t2f";
                    break;
                case GameFamily.Bmdm:
                    gameName = "bmdm";
                    break;
                case GameFamily.Cscz:
                    gameName = "cscz";
                    break;
                case GameFamily.Insurgency:
                    gameName = "ins";
                    break;
                case GameFamily.JustCause2:
                    gameName = "jc2";
                    break;
            }

            return gameName;
        }

        private ITaskHandler GetChangeStatusGameServerTaskHandler(TaskV2 task, GameServer gameServer)
        {
            var param = GetGameHostConnectParam(gameServer);
            IGameHost provider = GameServerFactory.Instance.Get(param);
            var handler = new ChangeGameServerStatusTaskHandler(task, provider);

            return handler;
        }
        private ITaskHandler GetDeleteGameServerTaskHandler(TaskV2 task, GameServer gameServer)
        {
            throw new NotFiniteNumberException();
        }

        private ConnectParam GetGameHostConnectParam(GameServer gameServer)
        {
            ConnectParam param = new ConnectParam
            {
                FamilyGame = gameServer.GameServerTariff.Game.Family,
                SshIp = gameServer.GameHost.ServerAddress,
                GameName = MapGameName(gameServer.GameServerTariff.Game.Family),
                SshPort = gameServer.GameHost.Port,
                SshPassword = gameServer.GameHost.Password,
                SshUserName = gameServer.GameHost.UserName,
                Path = "/host"//"/home/vncuser/host"
            };

            return param;
        }
        #endregion // Private methods
    }
}
