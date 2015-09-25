using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models;
using HyperVRemote;
using HyperVRemote.Source.Implementation;

namespace Crytex.Background
{
    class HyperVMonitorFactory : IHyperVMonitorFactory
    {
        public IHyperVProvider CreateHyperVProvider(HyperVHost host)
        {
            var configuration = new HyperVConfiguration(host.UserName, host.Password, host.Host, "testNameSpace");
            var hyperVProvider = new HyperVProvider(configuration);

            return hyperVProvider;
        }
    }
}
