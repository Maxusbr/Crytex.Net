using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Project.Model.Models;

namespace Project.Service
{
    public interface ISender
    {
        void SendCommand(CreateVmTask task);
        void SendCommand(UpdateVmTask task);
        void SendCommand(StandartVmTask task);
    }
}
