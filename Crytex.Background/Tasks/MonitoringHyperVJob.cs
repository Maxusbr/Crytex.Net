using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Crytex.Notification;
using System.Threading.Tasks;
using Crytex.Background.Monitor;
using Crytex.Background.Monitor.HyperV;
using Crytex.Model.Models;
using Crytex.Service.IService;
using HyperVRemote;

namespace Crytex.Background.Tasks
{
    using System;
    using Quartz;
    using Crytex.Background.Monitor.HyperV;

    [DisallowConcurrentExecution]
    public class MonitoringHyperVJob : IJob
    {
        private IHyperVMonitorFactory _hyperVMonitorFactory { get; set; }
        private IStateMachineService _stateMachine { get; set; }
        private IUserVmService _userVm { get; set; }
        private ISystemCenterVirtualManagerService _systemCenter { get; set; }

        public MonitoringHyperVJob (IHyperVMonitorFactory hyperVMonitorFactory,
            IStateMachineService stateMachine, 
            IUserVmService userVm,
            ISystemCenterVirtualManagerService systemCenter)
        {
            this._hyperVMonitorFactory = hyperVMonitorFactory;
            this._stateMachine = stateMachine;
            this._userVm = userVm;
            this._systemCenter = systemCenter;
        }

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("HyperV monitoring job");
            List<UserVm> allVMs = _userVm.GetAllVmsHyperV().ToList();
            var hosts = _systemCenter.GetAllHyperVHosts().ToList();
            List<Task> tasks = new List<Task>(hosts.Count());

            foreach (var host in hosts)
            {
                Task task = Task.Factory.StartNew(()=>GetVmInfo(host, allVMs));
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
        }

        public void GetVmInfo(HyperVHost host, List<UserVm> allVMs)
        {
            var hyperVMonitor = _hyperVMonitorFactory.CreateHyperVMonitor(host);
            var hostVms = allVMs.Where(v=>v.VirtualizationType == TypeVirtualization.HyperV && v.HyperVHostId == host.Id);

            foreach (var vm in hostVms)
            {
                var stateData = hyperVMonitor.GetVmByName(vm.Name);

                StateMachine vmState = new StateMachine
                {
                    CpuLoad = stateData.CPUUsage,
                    RamLoad = stateData.MemoryAssigned,
                    UpTime = stateData.Uptime,
                    Date = DateTime.UtcNow,
                    VmId = vm.Id
                };
                var newState = _stateMachine.CreateState(vmState);
            }
        }
    }
}