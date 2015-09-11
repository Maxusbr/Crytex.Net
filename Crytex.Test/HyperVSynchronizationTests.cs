using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Crytex.Test.FakeImplementations;
using Crytex.Model.Models;
using System.Collections.Generic;
using Crytex.Background.Tasks;
using Quartz;

namespace Crytex.Test
{
    [TestClass]
    public class HyperVSynchronizationTests
    {
        private FakeSystemCenterRemoteVirtualManagerService _fakeRemoteSerivce;
        private FakeSystemCenterVirtualManagerService _fakeDbService;

        [TestInitialize]
        public void Init()
        {
            this._fakeDbService = new FakeSystemCenterVirtualManagerService();
            this._fakeRemoteSerivce = new FakeSystemCenterRemoteVirtualManagerService();
        }

        [TestMethod]
        public void DeleteHostTest()
        {
            var firstHostAddr = "HOST_FIRST";
            var host1 = new HyperVHost
            {
                Id = Guid.NewGuid(),
                Host = firstHostAddr,
                Resources = new List<HyperVHostResource>(),
                Valid = true
            };
            var secondHostAddr = "HOST_SECOND";
            var host2 = new HyperVHost
            {
                Id = Guid.NewGuid(),
                Host = secondHostAddr,
                Resources = new List<HyperVHostResource>(),
                Valid = true
            };
            var manager = new SystemCenterVirtualManager
            {
                HyperVHosts = new List<HyperVHost> { host1, host2 }
            };
            this._fakeDbService.StoredManagers.Add(manager);
            this._fakeRemoteSerivce.RemoteHosts = new List<HyperVHost> { host1 };

            var job = new HyperVSynchronizationJob(this._fakeRemoteSerivce, this._fakeDbService);
            IJobExecutionContext context = null;
            job.Execute(context);

            var synchronizedHosts = this._fakeDbService.GetAll().Single().HyperVHosts;
            Assert.IsTrue(synchronizedHosts.Single(h => h.Host == firstHostAddr).Valid == true);
            Assert.IsTrue(synchronizedHosts.Single(h => h.Host == secondHostAddr).Valid == false);
        }

        [TestMethod]
        public void AddNewHostTest()
        {
            var manager = new SystemCenterVirtualManager
            {
                Id = Guid.NewGuid()
            };
            var firstHostAddr = "HOST_FIRST";
            var host1 = new HyperVHost
            {
                Id = Guid.NewGuid(),
                Host = firstHostAddr,
                Valid = true,
                Resources = new List<HyperVHostResource>(),
                SystemCenterVirtualManagerId = manager.Id
            };
            var secondHostAddr = "HOST_SECOND";
            var host2 = new HyperVHost
            {
                Id = Guid.NewGuid(),
                Host = secondHostAddr,
                Valid = true,
                Resources = new List<HyperVHostResource>(),
                SystemCenterVirtualManagerId = manager.Id
            };
            manager.HyperVHosts = new List<HyperVHost> { host1 };
            
            this._fakeDbService.StoredManagers.Add(manager);
            this._fakeRemoteSerivce.RemoteHosts = new List<HyperVHost> { host1, host2 };

            var job = new HyperVSynchronizationJob(this._fakeRemoteSerivce, this._fakeDbService);
            IJobExecutionContext context = null;
            job.Execute(context);

            var synchronizedHosts = this._fakeDbService.GetAll().Single().HyperVHosts;
            Assert.IsTrue(synchronizedHosts.Single(h => h.Host == secondHostAddr).DateAdded.Day == DateTime.UtcNow.Day);
        }

        [TestMethod]
        public void DeleteResourceTest()
        {
            var manager = new SystemCenterVirtualManager
            {
                Id = Guid.NewGuid()
            };
            var hostResource = new HyperVHostResource
            {
                Id = Guid.NewGuid(),
                ResourceType = HostResourceType.Hdd,
                Valid = true,
                Value = "10"
            };
            var firstHostAddr = "HOST_FIRST";
            var remoteHost = new HyperVHost
            {
                Id = Guid.NewGuid(),
                Host = firstHostAddr,
                Valid = true,
                Resources = new List<HyperVHostResource>(),
                SystemCenterVirtualManagerId = manager.Id
            };
            var dbHost = new HyperVHost
            {
                Id = remoteHost.Id,
                Host = firstHostAddr,
                Valid = true,
                Resources = new List<HyperVHostResource>{hostResource},
                SystemCenterVirtualManagerId = manager.Id
            };
            manager.HyperVHosts = new List<HyperVHost>{dbHost};

            this._fakeDbService.StoredManagers.Add(manager);
            this._fakeRemoteSerivce.RemoteHosts.Add(remoteHost);
            
            var job = new HyperVSynchronizationJob(this._fakeRemoteSerivce, this._fakeDbService);
            IJobExecutionContext context = null;
            job.Execute(context);

            var synchronizedHosts = this._fakeDbService.GetAll().Single().HyperVHosts;
            Assert.IsTrue(synchronizedHosts.Single().Resources.Single().Valid == false);

        }

        [TestMethod]
        public void AddNewResourceTest()
        {
            var manager = new SystemCenterVirtualManager
            {
                Id = Guid.NewGuid()
            };
            var hostResource = new HyperVHostResource
            {
                Id = Guid.NewGuid(),
                ResourceType = HostResourceType.Hdd,
                Valid = true,
                Value = "10"
            };
            var firstHostAddr = "HOST_FIRST";
            var remoteHost = new HyperVHost
            {
                Id = Guid.NewGuid(),
                Host = firstHostAddr,
                Valid = true,
                Resources = new List<HyperVHostResource> { hostResource },
                SystemCenterVirtualManagerId = manager.Id
            };
            hostResource.HyperVHostId = remoteHost.Id;
            var dbHost = new HyperVHost
            {
                Id = remoteHost.Id,
                Host = firstHostAddr,
                Resources = new List<HyperVHostResource>(),
                Valid = true,
                SystemCenterVirtualManagerId = manager.Id
            };
            manager.HyperVHosts = new List<HyperVHost> { dbHost };

            this._fakeDbService.StoredManagers.Add(manager);
            this._fakeRemoteSerivce.RemoteHosts.Add(remoteHost);

            var job = new HyperVSynchronizationJob(this._fakeRemoteSerivce, this._fakeDbService);
            IJobExecutionContext context = null;
            job.Execute(context);

            var synchronizedHosts = this._fakeDbService.GetAll().Single().HyperVHosts;
            Assert.IsTrue(synchronizedHosts.Single().Resources.Single().UpdateDate.Day == DateTime.UtcNow.Day);
        }
    }
}
