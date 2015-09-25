using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models;
using Crytex.Notification;

namespace Crytex.Background
{
    interface IHyperVMonitor
    {
        StateMachine GetStateMachine(Guid vmId);
    }
}
