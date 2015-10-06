using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Service.IService
{
    public interface IVmWareVCenterService
    {
        VmWareVCenter CreateVCenter(VmWareVCenter vCenter);

        VmWareVCenter GetVCenterById(int id);

        IEnumerable<VmWareVCenter> GetAllVCenters();

        void UpdateVCenter(int id, VmWareVCenter vCenter);
    }
}
