using System;
using Microsoft.AspNet.SignalR;
using Sample.Service.IService;

namespace Crytex.Web.Hubs
{
    public class SampleHub : Hub
    {
        private readonly IMessageService _messageService;

        public SampleHub(IMessageService messageService)
        {
            _messageService = messageService;
        }


        public void SendMessage(String message)
        {
            Clients.All.Send(message);
            _messageService.LogMessage(message);
        }
    }
}