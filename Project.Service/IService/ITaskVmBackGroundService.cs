using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Project.Service.Model;

namespace Project.Service.IService
{
    public interface ITaskVmBackGroundService
    {

        void CreateVm(CreateVmOption createVmOption);
        void RemoveVm(RemoveVmOption removeVm);
        void UpdateVmOption(UpdateVmOption updateVmOption);
    }
}
