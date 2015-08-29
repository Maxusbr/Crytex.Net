using System.Collections.Generic;
using Crytex.Model.Models;

namespace Sample.Service.IService
{
    public interface IMessageService
    {
        void LogMessage(string message);
        List<Message> GetAll();
    }
}
