using System.Collections.Generic;
using System.Linq;
using Crytex.Notification;
using System.Threading.Tasks;
using Crytex.Model.Models;
using HyperVRemote;

namespace Crytex.Background.Tasks
{
    using System;
    using Quartz;

    public class MonitoringJob: IJob
    {
        private INotificationManager _notificationManager { get; set; }
        private IHyperVMonitorFactory _hyperVMonitorFactory { get; set; }

        public MonitoringJob(INotificationManager notificationManager,IHyperVMonitorFactory hyperVMonitorFactory)
        {
            this._hyperVMonitorFactory = hyperVMonitorFactory;
            this._notificationManager = notificationManager;
        }

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("It's monitoring job!");
            var VMList =_notificationManager.GetVMs();
            List<Task> tasks = new List<Task>(VMList.Count());
            foreach (UserVm VM in VMList)
            {
                Task task = Task.Factory.StartNew(() => GetVmInfo(VM));
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
        }

        private void GetVmInfo(UserVm vm)
        {
            BaseTask task = new BaseTask
            {
                StatusTask = StatusTask.Start,
                Virtualization = TypeVirtualization.HyperV,
                UserId = vm.UserId
            };
            HyperVHost host = new HyperVHost
            {
                Id = Guid.NewGuid(),
                Host = "Test_Host",
                Valid = true,
                UserName = "AdminUser",
                Password = "ADUe48j+601YsRBoFODIbUp5zruU5LCs3ESnMDAp2b8rnI2ABD3uwid8ApWP96prmg==",
                DateAdded = DateTime.Now,
                SystemCenterVirtualManagerId = Guid.NewGuid(),
                Deleted = false
            };
            var machine = _hyperVMonitorFactory.CreateHyperVProvider(task, host).GetVmByName(vm.Name);
            StateMachine vmData = new StateMachine
            {
                CpuLoad = machine.CPUUsage,
                RamLoad = machine.MemoryAssigned,
                Date = DateTime.Now
            };
            _notificationManager.SendVmMessage(vm.Id, vmData);
        }
    }
}