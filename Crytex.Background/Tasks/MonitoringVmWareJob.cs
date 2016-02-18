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
using VmWareRemote.Model;

namespace Crytex.Background.Tasks
{
    using System;
    using Quartz;
    using Crytex.Background.Monitor.Vmware;

    [DisallowConcurrentExecution]
    public class MonitoringVmWareJob : IJob
    {
        private IVmWareMonitorFactory _vmWareMonitorFactory { get; set; }
        private IStateMachineService _stateMachine { get; set; }
        private IUserVmService _userVm { get; set; }
        private IVmWareVCenterService _vCenter { get; set; }

        public MonitoringVmWareJob(IVmWareMonitorFactory vmWareMonitorFactory,
            IStateMachineService stateMachine,
            IUserVmService userVm,
            IVmWareVCenterService vCenter)
        {
            this._vmWareMonitorFactory = vmWareMonitorFactory;
            this._stateMachine = stateMachine;
            this._userVm = userVm;
            this._vCenter = vCenter;
        }

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Vmware monitoring job");
            List<UserVm> allVMs = _userVm.GetAllVmsVmWare().ToList();
            var vCenters = _vCenter.GetAllVCenters().ToList();
            List<Task> tasks = new List<Task>(vCenters.Count());

            foreach (var vCenter in vCenters)
            {
                Task task = Task.Factory.StartNew(() => GetVmInfo(vCenter, allVMs));
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
        }

        public void GetVmInfo(VmWareVCenter vCenter, List<UserVm> allVMs)
        {
            var vmWareMonitor = _vmWareMonitorFactory.CreateVmWareVMonitor(vCenter);
            var vCenterVms = allVMs.Where(v => v.VirtualizationType == TypeVirtualization.VmWare && v.VmWareCenterId == vCenter.Id);

            foreach (var vm in vCenterVms)
            {
                var stateData = vmWareMonitor.GetVmByName(vm.Name);

                StateMachine vmState = new StateMachine
                {
                    CpuLoad = Convert.ToInt32(stateData.CpuUsage),
                    RamLoad = Convert.ToInt32(stateData.RamUsage),
                    UpTime = TimeSpan.FromSeconds(stateData.Uptime.Value),
                    Date = DateTime.UtcNow,
                    VmId = vm.Id
                };
                var newState = _stateMachine.CreateState(vmState);
            }
        }
    }
}