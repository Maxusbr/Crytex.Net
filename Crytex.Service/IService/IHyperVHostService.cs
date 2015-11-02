using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Service.IService
{
    public interface IHyperVHostService
    {
        HyperVHost CreateHyperVHost(HyperVHost hyperVHost);

        HyperVHost GetHyperVById(int id);

        IEnumerable<HyperVHost> GetAllHyperVHosts();

        void UpdateHyperVHost(HyperVHost hyperVHost);
    }
}
