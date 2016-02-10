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
            UnityConfig.Configure();
            var taskManager = UnityConfig.Resolve<ITaskQueuePoolManager>();
        }
    }
}
