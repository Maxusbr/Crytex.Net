using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models;
using HyperVRemote;

namespace Crytex.Background
{
    public interface IHyperVMonitorFactory
    {
        IHyperVProvider CreateHyperVProvider(BaseTask task, HyperVHost host);
    }
}
