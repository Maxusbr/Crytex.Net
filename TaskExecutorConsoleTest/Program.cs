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
using Crytex.Service.IService;
using VmWareRemote.Interface;

namespace TaskExecutorConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            UnityConfig.Configure();
            var taskService = UnityConfig.Resolve<ITaskV2Service>();
            var tasks = taskService.GetPendingTasks(new TypeTask[] {TypeTask.CreateVm, TypeTask.UpdateVm,});
            foreach (var taskV2 in tasks)
            {
                Console.WriteLine($"Task id ={taskV2.Id}");
            }
        }
    }
}
