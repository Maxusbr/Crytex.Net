using System;
using Crytex.Background.Monitor;
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
        private readonly IVmMonitorFactory _monitorFactory;


        public MonitoringVmActiveJob(INotificationManager notificationManager, IVmMonitorFactory monitorFactory, IUserVmService userVmService)
        {
            this._notificationManager = notificationManager;
            _monitorFactory = monitorFactory;
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
            var monitor = _monitorFactory.GetHyperVMonitor(vm.HyperVHost);
            var info = monitor.GetMachineState(vm.Id.ToString());

            StateMachine vmState = new StateMachine
            {
                CpuLoad = Convert.ToInt32(info.CpuUsage),
                RamLoad = Convert.ToInt32(info.RamUsage),
                Date = DateTime.UtcNow,
                VmId = vm.Id
            };

            this._notificationManager.SendVmMessage(vm.Id, vmState);
        }

        private void SendVmWareStateMessage(UserVm vm)
        {
            var monitor = _monitorFactory.GetVmWareMonitor(vm.VmWareCenter);
            var info = monitor.GetMachineState(vm.Id.ToString());

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
