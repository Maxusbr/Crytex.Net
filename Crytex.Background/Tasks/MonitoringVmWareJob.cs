using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Crytex.Notification;
using System.Threading.Tasks;
using Crytex.Background.Monitor;
using Crytex.Background.Monitor.Vmware;
using Crytex.Model.Models;
using Crytex.Service.IService;
using HyperVRemote;

namespace Crytex.Background.Tasks
{
    using System;
    using Quartz;
    using Crytex.Background.Monitor.Vmware;

    public class MonitoringVmWareJob : IJob
    {
        private INotificationManager _notificationManager { get; set; }
        private IVmWareMonitorFactory _vmWareMonitorFactory { get; set; }
        private IStateMachineService _stateMachine { get; set; }
        private IUserVmService _userVm { get; set; }
        private IVmWareVCenterService _vCenter { get; set; }

        public MonitoringVmWareJob(INotificationManager notificationManager,
            IVmWareMonitorFactory vmWareMonitorFactory,
            IStateMachineService stateMachine,
            IUserVmService userVm,
            IVmWareVCenterService vCenter)
        {
            this._vmWareMonitorFactory = vmWareMonitorFactory;
            this._notificationManager = notificationManager;
            this._stateMachine = stateMachine;
            this._userVm = userVm;
            this._vCenter = vCenter;
        }

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("It's monitoring job!");
            List<Guid> vmActiveList = _notificationManager.GetVMs();
            List<UserVm> allVMs = _userVm.GetAllVmsHyperV().ToList();
            var vCenters = _vCenter.GetAllVCenters().ToList();
            List<Task> tasks = new List<Task>(vCenters.Count());

            foreach (var vCenter in vCenters)
            {
                Task task = Task.Factory.StartNew(() => GetVmInfo(vCenter, allVMs, vmActiveList));
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
        }

        public void GetVmInfo(VmWareVCenter vCenter, List<UserVm> allVMs, List<Guid> vmActiveList)
        {
            var vmWareMonitor = _vmWareMonitorFactory.CreateVmWareVMonitor(vCenter);
            var hostVms = allVMs.Where(v => v.VurtualizationType == TypeVirtualization.WmWare && v.VmWareCenterId == vCenter.Id);

            foreach (var vm in hostVms)
            {
                var stateData = vmWareMonitor.GetVmByName(vm.Name);

                StateMachine vmState = new StateMachine
                {
                    CpuLoad = Convert.ToInt32(stateData.CpuUsage),
                    RamLoad = Convert.ToInt32(stateData.RamUsage),
                    Date = DateTime.UtcNow,
                    VmId = vm.Id
                };
                var newState = _stateMachine.CreateState(vmState);
                if (vmActiveList.Contains(vm.Id))
                {
                    _notificationManager.SendVmMessage(vm.Id, newState);
                }
            }
        }
    }
}