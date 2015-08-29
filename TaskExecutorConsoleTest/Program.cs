using Crytex.ExecutorTask;
using Crytex.ExecutorTask.TaskHandler;
using Crytex.Data.Infrastructure;
using Crytex.Data.Repository;
using Crytex.Service.Service;
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
            var userVmRepository = new UserVmRepository(dbFactory);
            var serverTempalateRepo = new ServerTemplateRepository(dbFactory);
            var fileDescriptorRepo = new FileDescriptorRepository(dbFactory);
            var unitOfWork = new UnitOfWork(dbFactory);
            var service = new TaskVmService(unitOfWork, createRepo, upRepo, standartRepo, userVmRepository, serverTempalateRepo, fileDescriptorRepo);
            var handlerManager = new TaskHandlerManager(service);
            var taskManager = new TaskManager(handlerManager);

            taskManager.Run();

            Console.ReadLine();
        }
    }
}
