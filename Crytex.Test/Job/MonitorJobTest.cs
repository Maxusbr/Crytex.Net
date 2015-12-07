using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Management.Automation;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using Crytex.Background;
using Crytex.Background.Monitor;
using Crytex.Background.Monitor.HyperV;
using Crytex.Background.Tasks;
using Crytex.Core.Service;
using Crytex.Data;
using Crytex.Model.Models;
using Crytex.Notification;
using Crytex.Service.IService;
using Crytex.Web;
using Crytex.Web.Controllers.Api;
using Crytex.Web.Mappings;
using Crytex.Web.Models.JsonModels;
using Crytex.Web.Service;
using NSubstitute;
using NSubstitute.Extensions;
using NUnit.Framework;
using Crytex.Notification.Service;
using HyperVRemote;
using HyperVRemote.Source.Implementation;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using PagedList;
using Quartz;
using static NUnit.Framework.Assert;

namespace Crytex.Test.Notification.Hub
{

    [TestFixture]
    class MonitoringJobTest
    {
        INotificationManager _notificationManager { get; set; }
        IHyperVMonitorFactory _hyperVMonitorFactory { get; set; }
        IStateMachineService _stateMachine { get; set; }
        IUserVmService _userVm { get; set; }
        ISystemCenterVirtualManagerService _systemCenter { get; set; }
        MonitoringJob _monitoringJob { get; set; }


        [SetUp]
        public void Init()
        {
            _notificationManager = Substitute.For<INotificationManager>();
            _hyperVMonitorFactory = Substitute.For<IHyperVMonitorFactory>();
            _stateMachine = Substitute.For<IStateMachineService>();
            _userVm = Substitute.For<IUserVmService>();
            _systemCenter = Substitute.For<ISystemCenterVirtualManagerService>();

            _monitoringJob = Substitute.For<MonitoringJob>(_notificationManager,
                _hyperVMonitorFactory,
                _stateMachine,
                _userVm,
                _systemCenter);
        }

        [Test]
        public void ExecuteTest()
        {
            _notificationManager.GetVMs().Returns(new List<Guid>());
            _userVm.GetAllVmsHyperV().Returns(Enumerable.Empty<UserVm>());
            
            var hyperVHosts = new List<HyperVHost>();
            hyperVHosts.Add(new HyperVHost());
            hyperVHosts.Add(new HyperVHost());

            _systemCenter.GetAllHyperVHosts().Returns(hyperVHosts.Cast<HyperVHost>());

            IJobExecutionContext context = Substitute.For<IJobExecutionContext>();
            _monitoringJob.Execute(context);
            _monitoringJob.Received(hyperVHosts.Count()).GetVmInfo(new HyperVHost(), new List<UserVm>(), new List<Guid>());
        }

        [Test]
        public void GetVmInfoTest()
        {
            HyperVHost host = new HyperVHost
            {
                Id = Guid.NewGuid()
            };
            IHyperVProvider provider = Substitute.For<IHyperVProvider>();
            IHyperVMonitor monitor = Substitute.For<IHyperVMonitor>();
            _hyperVMonitorFactory.CreateHyperVMonitor(host).Returns(monitor);

            List<UserVm> allHostVMs = new List<UserVm>();
            var userVm = new UserVm
            {
                HyperVHostId = host.Id,
                VirtualizationType = TypeVirtualization.HyperV
            };
            userVm.Name = "First";
            userVm.Id = Guid.NewGuid();
            allHostVMs.Add(userVm);

            userVm.Name = "Second";
            userVm.Id = Guid.NewGuid();
            allHostVMs.Add(userVm);

            PSObject name = new PSObject();
            name.Properties.Add(new PSNoteProperty("CPUUsage", 1));
            name.Properties.Add(new PSNoteProperty("MemoryAssigned", Convert.ToInt64(2)));
            name.Properties.Add(new PSNoteProperty("Name", "nameString"));
            name.Properties.Add(new PSNoteProperty("State", "State"));
            name.Properties.Add(new PSNoteProperty("Status", "Status"));
            name.Properties.Add(new PSNoteProperty("Uptime", TimeSpan.MinValue));

            HyperVMachine machine = new HyperVMachine(name);
            monitor.GetVmByName("name").ReturnsForAnyArgs(machine);

            _stateMachine.CreateState(new StateMachine{Id = 0}).Returns(new StateMachine { Id = 1 });
            List<Guid> vmActiveList = new List<Guid>
            {
                allHostVMs[0].Id,
                allHostVMs[1].Id
            };

            _monitoringJob.GetVmInfo(host, allHostVMs, vmActiveList);
            _notificationManager.ReceivedWithAnyArgs(2).SendVmMessage(allHostVMs[0].Id, new StateMachine());
        }

    }
}
