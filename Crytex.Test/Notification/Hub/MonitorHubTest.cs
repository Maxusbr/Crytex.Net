using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using PagedList;
using static NUnit.Framework.Assert;

namespace Crytex.Test.Notification.Hub
{
    public interface IClientContract
    {
        void RecieveVmMessage(StateMachine stateMachine);
        int Testtest();
    }

    [TestFixture]
    class MonitorHubTest
    {
        MonitorHub _monitorHub { get; set; }
        UserInfo _userInfo { get; set; }
        IUserVmService _userVmService { get; set; }
        HubCallerContext _context { get; set; }
        INotifyProvider _notifyProvider { get; set; }
        IHubCallerConnectionContext<dynamic> _client { get; set; }
        IClientContract _clientContract { get; set; }

        [SetUp]
        public void Init()
        {
            _userVmService = Substitute.For<IUserVmService>();
            _notifyProvider = Substitute.For<INotifyProvider>();
            _monitorHub = Substitute.For<MonitorHub>(_userVmService);
            // _monitorHub = new MonitorHub(_userVmService);
            _userInfo = new UserInfo() { UserId = "UserId" };
            IRequest request = Substitute.For<IRequest>();
            _context = new HubCallerContext(request, "connectionId");
            _client = Substitute.For<IHubCallerConnectionContext<dynamic>>();
            _clientContract = Substitute.For<IClientContract>();
            //_client.ReturnsForAll(_clientContract);
            //SubstituteExtensions.Returns(_client.All, _clientContract);
            _monitorHub.Clients = _client;
            _notifyProvider.GetUserId(_context).Returns(_userInfo.UserId);
            _monitorHub.Context = _context;
            _monitorHub.NotifyProvider = _notifyProvider;
        }

        [Test]
        public void MonitorSubscribeSuccess()
        {
            UserVm VM = new UserVm
            {
                Id = Guid.NewGuid(),
                UserId = _userInfo.UserId
            };
            _userVmService.GetVmById(VM.Id).Returns(VM);
            _monitorHub.OnConnected();

            _monitorHub.Subscribe(VM.Id.ToString());
            List<Guid> VMs = _monitorHub.GetVMs();
            NotNull(VMs);
            AreEqual(VMs.FirstOrDefault(), VM.Id);
        }

        [Test]
        public void MonitorSubscribeBad()
        {
            UserVm VM = new UserVm
            {
                Id = Guid.NewGuid(),
                UserId = _userInfo.UserId + "NotValidUSer"
            };
            _userVmService.GetVmById(VM.Id).Returns(VM);
            _monitorHub.OnConnected();

            _monitorHub.Subscribe(VM.Id.ToString());
            List<Guid> VMs = _monitorHub.GetVMs();
            IsEmpty(VMs);
        }

        //[Test]
        //public void MonitorSendVmMessage()
        //{
        //    UserVm VM = new UserVm
        //    {
        //        Id = Guid.NewGuid(),
        //        UserId = _userInfo.UserId
        //    };
        //    _userVmService.GetVmById(VM.Id).Returns(VM);
        //    _monitorHub.OnConnected();

        //    _monitorHub.Sybscribe(VM.Id.ToString());
        //    List<Guid> VMs = _monitorHub.GetVMs();
        //    NotNull(VMs);
        //    AreEqual(VMs.FirstOrDefault(), VM.Id);
        //    var stateMachine = new StateMachine();
            
        //    //_client.ReturnsForAll(_clientContract);
        //    SubstituteExtensions.Returns(_client.All, _clientContract);
        //    _monitorHub.SendVmMessage(VM.Id, stateMachine);
            
        //    // _monitorHub.Clients.Client(_context.ConnectionId).Return

        //    _clientContract.ReceivedWithAnyArgs(1).RecieveVmMessage(stateMachine);
        //}
        


    }
}
