//using System.Collections.Generic;
//using System.Linq;
//using System.Management.Automation;
//using Crytex.Notification;
//using System.Threading.Tasks;
//using Crytex.Background.Monitor;
//using Crytex.Model.Models;
//using Crytex.Service.IService;
//using HyperVRemote;

//namespace Crytex.Background.Tasks
//{
//    using System;
//    using Quartz;

//    public class MonitoringVmWareJob: IJob
//    {
//        private INotificationManager _notificationManager { get; set; }
//        private IMonitorFactory _monitorFactory { get; set; }
//        private IStateMachineService _stateMachine { get; set; }
//        private IUserVmService _userVm { get; set; }
//        private ISystemCenterVirtualManagerService _systemCenter { get; set; }

//        public MonitoringVmWareJob(INotificationManager notificationManager,
//            IMonitorFactory MonitorFactory,
//            IStateMachineService stateMachine, 
//            IUserVmService userVm,
//            ISystemCenterVirtualManagerService systemCenter)
//        {
//            this._monitorFactory = MonitorFactory;
//            this._notificationManager = notificationManager;
//            this._stateMachine = stateMachine;
//            this._userVm = userVm;
//            this._systemCenter = systemCenter;
//        }

//        public void Execute(IJobExecutionContext context)
//        {
//            Console.WriteLine("It's monitoring job!");
//            List<Guid> vmActiveList = _notificationManager.GetVMs();
//            List<UserVm> allVMs = _userVm.GetAllVmsHyperV().ToList();
//            var hosts = _systemCenter.GetAllHyperVHosts().ToList();
//            List<Task> tasks = new List<Task>(hosts.Count());

//            foreach (var host in hosts)
//            {
//                Task task = Task.Factory.StartNew(()=>GetVmInfo(host, allVMs, vmActiveList));
//                tasks.Add(task);
//            }
//            Task.WaitAll(tasks.ToArray());
//        }

//        public void GetVmInfo(VmWareHost host, List<UserVm> allVMs, List<Guid> vmActiveList)
//        {
//            var vmWareMonitor = _monitorFactory.CreateVmWareVMonitor(host);
//            var hostVms = allVMs.Where(v=>v.VurtualizationType == TypeVirtualization.WmWare && v.HyperVHostId == host.Id);

//            foreach (var vm in hostVms)
//            {
//                var stateData = hyperVMonitor.GetVmByName(vm.Name);

//                StateMachine vmState = new StateMachine
//                {
//                    CpuLoad = stateData.CPUUsage,
//                    RamLoad = stateData.MemoryAssigned,
//                    Date = DateTime.UtcNow,
//                    VmId = vm.Id
//                };
//                var newState = _stateMachine.CreateState(vmState);
//                if (vmActiveList.Contains(vm.Id))
//                {
//                    _notificationManager.SendVmMessage(vm.Id, newState);
//                }
//            }
//        }
    }
}