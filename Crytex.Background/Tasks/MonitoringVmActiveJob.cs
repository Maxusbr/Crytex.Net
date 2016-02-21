using System;
using Crytex.Background.Monitor.HyperV;
using Crytex.Background.Monitor.Vmware;
using Crytex.Notification;
using Quartz;
using Crytex.Service.IService;
using Crytex.Model.Models;

namespace Crytex.Background.Tasks
{
    [DisallowConcurrentExecution]
    public class MonitoringVmActiveJob : IJob
    {
        private readonly IUserVmService _userVmService;
        private readonly INotificationManager _notificationManager;
        private readonly IVmWareMonitorFactory _vmWareMonitorFactory;
        private readonly IHyperVMonitorFactory _hyperVMonitorFactory;

        public MonitoringVmActiveJob(INotificationManager notificationManager, IVmWareMonitorFactory vmWareMonitorFactory,
            IHyperVMonitorFactory hyperVMonitorFactory, IUserVmService userVmService)
        {
            this._hyperVMonitorFactory = hyperVMonitorFactory;
            this._vmWareMonitorFactory = vmWareMonitorFactory;
            this._notificationManager = notificationManager;
            this._userVmService = userVmService;
        }

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("MonitoringVmActiveJob");
            var vmActiveGuidList = this._notificationManager.GetVMs();
            var vms = this._userVmService.GetVmsByIds(vmActiveGuidList);

            foreach (var vm in vms)
            {
                switch (vm.VirtualizationType)
                {
                    case TypeVirtualization.VmWare:
                        this.SendVmWareStateMessage(vm);
                        break;
                    case TypeVirtualization.HyperV:
                        this.SendHyperVStateMessage(vm);
                        break;
                }
            }
        }

        private void SendHyperVStateMessage(UserVm vm)
        {
            var monitor = this._hyperVMonitorFactory.CreateHyperVMonitor(vm.HyperVHost);
            var info = monitor.GetVmByName(vm.Id.ToString());

            StateMachine vmState = new StateMachine
            {
                CpuLoad = Convert.ToInt32(info.CPUUsage),
                RamLoad = Convert.ToInt32(info.MemoryAssigned),
                Date = DateTime.UtcNow,
                VmId = vm.Id
            };

            this._notificationManager.SendVmMessage(vm.Id, vmState);
        }

        private void SendVmWareStateMessage(UserVm vm)
        {
            var monitor = this._vmWareMonitorFactory.CreateVmWareVMonitor(vm.VmWareCenter);
            var info = monitor.GetVmByName(vm.Id.ToString());

            StateMachine vmState = new StateMachine
            {
                CpuLoad = Convert.ToInt32(info.CpuUsage),
                RamLoad = Convert.ToInt32(info.RamUsage),
                Date = DateTime.UtcNow,
                VmId = vm.Id
            };

            _notificationManager.SendVmMessage(vm.Id, vmState);
        }
    }
}
