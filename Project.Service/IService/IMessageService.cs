using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Model.Models;

namespace Sample.Service.IService
{
    public interface IMessageService
    {
        void LogMessage(string message);
        List<Message> GetAll();
    }
}
