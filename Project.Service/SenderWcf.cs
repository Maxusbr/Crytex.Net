using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Model.Models;

namespace Project.Service
{
    public class SenderWcf : ISender
    {
        public void SendCommand(CreateVmTask task)
        {
            var sender= new Executor.ReceiverServiceClient();
            sender.CreateVm(task.Id);
        }

        public void SendCommand(UpdateVmTask task)
        {
            throw new NotImplementedException();
        }

        public void SendCommand(StandartVmTask task)
        {
            throw new NotImplementedException();
        }
    }
}
