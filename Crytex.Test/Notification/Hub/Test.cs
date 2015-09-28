using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using NSubstitute;
using NUnit.Framework;

namespace Crytex.Test.Notification.Hub
{
    public class ProjectsHub : Microsoft.AspNet.SignalR.Hub
    {
        public void AddProject(string id)
        {
            Clients.All.AddProject(id);
        }
    }

    [TestFixture]
    public class ProjectsHubTests
    {
        // Operations that clients might receive
        // This interface is in place in order to mock the
        // dynamic object used in SignalR
        
        public interface ISignals
        {
            void AddProject(string id);
        }

        [Test]
        public void AddProject_Broadcasts()
        {
            // Arrange
            ProjectsHub hub = new ProjectsHub();
            IHubCallerConnectionContext<dynamic> clients =
                    Substitute.For<IHubCallerConnectionContext<dynamic>>();
            ISignals signals = Substitute.For<ISignals>();
            //SubstituteExtensions.Returns(clients.All, signals);
            hub.Clients = clients;
            //Substitute.Returns(clients.Client(Arg.Any<string>()), all);
            // Act
            hub.AddProject("id");

            // Assert
            signals.Received(1).AddProject("id");
        }
    }
}
