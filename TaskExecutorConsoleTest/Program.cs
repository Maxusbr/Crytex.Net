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
            var userName = "";
            var password = "";
            var server = "51.254.55.136";
            var vmWareProvider = new VmWareProvider(userName, password, server);
            var vmWareControl = new VmWareControl(vmWareProvider);

            //var createTask = new CreateVmTask
            //{
            //    Cpu = 2,
            //    Ram = 1
            //};
            
            //var name = vmWareControl.CreateVm(createTask);

            var updateTask = new UpdateVmTask
            {
                Ram = 2,
                Cpu = 2,
                VmId = new Guid("9619d6f3-5306-4ef6-bcb0-9bbe8af53b85")
            };

            vmWareControl.UpdateVm(updateTask);
        }
    }
}
