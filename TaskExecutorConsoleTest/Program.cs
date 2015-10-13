using Crytex.ExecutorTask;
using Crytex.ExecutorTask.TaskHandler;
using Crytex.Data.Infrastructure;
using Crytex.Data.Repository;
using Crytex.Service.Service;
using System;
using Crytex.Notification;
using Crytex.Notification.Senders.SigralRSender;
using Crytex.ExecutorTask.TaskHandler.VmWare;
using VmWareRemote.Implementations;
using Crytex.Model.Models;

namespace TaskExecutorConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var handlerFactory = new TaskHandlerFactory();
            //var defVCenter = new VmWareVCenter
            //{
            //    Name = "default",
            //    UserName = "administrator@vsphere.local",
            //    Password = "QwerT@12",
            //    ServerAddress = "51.254.55.136"
            //};

            //var updateTask = new TaskV2
            //{
            //    TypeTask = TypeTask.RemoveVm,
            //    Virtualization = TypeVirtualization.WmWare,
            //    ResourceId = new Guid("9619d6f3-5306-4ef6-bcb0-9bbe8af53b85")
            //};

            //var options = new RemoveVmOptions
            //    {
                    
            //    };
            //updateTask.SaveOptions<RemoveVmOptions>(options);

            //var handler = handlerFactory.GetVmWareHandler(updateTask, defVCenter);
            //handler.Execute();
        }
    }
}
