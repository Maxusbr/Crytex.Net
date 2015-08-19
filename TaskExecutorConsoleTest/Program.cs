﻿using Crytex.ExecutorTask;
using Crytex.ExecutorTask.alt;
using Crytex.ExecutorTask.Hyper_V;
using Crytex.ExecutorTask.VmWare;
using Project.Data.Infrastructure;
using Project.Data.Repository;
using Project.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskExecutorConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbFactory = new DatabaseFactory();
            var upRepo = new UpdateTaskVmRepository(dbFactory);
            var createRepo = new CreateVmTaskRepository(dbFactory);
            var standartRepo = new StandartVmTaskRepository(dbFactory);
            var unitOfWork = new UnitOfWork(dbFactory);
            var service = new TaskVmService(unitOfWork, createRepo, upRepo, standartRepo);
            var taskManager = new TaskManager(service);

            taskManager.Run();

            Console.ReadLine();
        }
    }
}
