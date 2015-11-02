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
using VmWareRemote.Model;
using Crytex.ExecutorTask.Config;
using VmWareRemote.Interface;

namespace TaskExecutorConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ExecutorTaskConfig();
            Console.WriteLine(config.GetHyperVTemplateDriveRoot());
            Console.WriteLine(config.GetHyperVVmDriveRoot());
            userName = "administrator@vsphere.local";
             password = "QwerT@12";
             server = "51.254.55.136";

            IVmWareProvider provider = new VmWareProvider(userName, password, server);
            var vmWareControl = new VmWareControl(provider);
            var task = new TaskV2();
            var createOptions = new CreateVmOptions
            {
                Cpu = 3,
                Hdd = 50,
                Ram = 8 * 1024
            };
            task.SaveOptions(createOptions);
            var template = new ServerTemplate
            {
                OperatingSystem = new Crytex.Model.Models.OperatingSystem
                {
                    ServerTemplateName = "TestFromWebClient"
                }
            };

            vmWareControl.CreateVm(task, template);
        }
    }
}
