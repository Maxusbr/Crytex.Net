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

    [TestFixture]
    class NotificationManagerTest
    {
        NotificationManager _notificationManager { get; set; }
        IEmailSender _emailSender { get; set; }
        IEmailInfoService _emailInfoService { get; set; }
        ISignalRSender _signalRSender { get; set; }

        [SetUp]
        public void Init()
        {
            _emailSender = Substitute.For<IEmailSender>();
            _emailInfoService = Substitute.For<IEmailInfoService>();
            _signalRSender = Substitute.For<ISignalRSender>();
            _notificationManager = Substitute.For<NotificationManager>(_emailSender,_emailInfoService,_signalRSender);
        }

        [Test]
        public void NotificationManagerGetVMs()
        {
            var testList = new List<Guid>();
            testList.Add(Guid.NewGuid());
            _signalRSender.GetVMs().Returns(testList);

            var listNotificationGuid = _notificationManager.GetVMs();
            _signalRSender.Received(1).GetVMs();
            IsNotEmpty(listNotificationGuid);
            That(listNotificationGuid, Is.EquivalentTo(testList));
        }

    }
}
