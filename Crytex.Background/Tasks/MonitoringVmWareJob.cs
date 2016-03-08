using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crytex.Background.Monitor;
using Crytex.Model.Models;
using Crytex.Service.IService;

namespace Crytex.Background.Tasks
{
    using System;
    using Quartz;

    [DisallowConcurrentExecution]
    public class MonitoringVmWareJob : IJob
    {
        private readonly IVmMonitorFactory _monitorFactory;
        private IStateMachineService _stateMachineService { get; set; }
        private IUserVmService _userVm { get; set; }
        private IVmWareVCenterService _vCenter { get; set; }

        public MonitoringVmWareJob(IVmMonitorFactory monitorFactory,
            IStateMachineService stateMachineService,
            IUserVmService userVm,
            IVmWareVCenterService vCenter)
        {
            _monitorFactory = monitorFactory;
            this._stateMachineService = stateMachineService;
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
            var vmWareMonitor = _monitorFactory.GetVmWareMonitor(vCenter);
            var vCenterVms = allVMs.Where(v => v.VirtualizationType == TypeVirtualization.VmWare && v.VmWareCenterId == vCenter.Id);

            foreach (var vm in vCenterVms)
            {
                var stateData = vmWareMonitor.GetMachineState(vm.Name);

                StateMachine vmState = new StateMachine
                {
                    CpuLoad = Convert.ToInt32(stateData.CpuUsage),
                    RamLoad = Convert.ToInt32(stateData.RamUsage),
                    UpTime = stateData.Uptime,
                    Date = DateTime.UtcNow,
                    VmId = vm.Id
                };
                _stateMachineService.CreateState(vmState);
            }
        }
    }
}