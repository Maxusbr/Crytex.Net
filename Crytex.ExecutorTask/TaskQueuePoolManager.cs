using Crytex.ExecutorTask.TaskHandler;
using Crytex.Service.IService;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using Crytex.Model.Models;

namespace Crytex.ExecutorTask
{
    public class TaskQueuePoolManager : ITaskQueuePoolManager
    {
        public TypeTask[] TaskTypes { get; set; }
        private List<UserTaskQueueManager> _taskQueueManagers = new List<UserTaskQueueManager>();
        private readonly ITaskV2Service _taskService;
        private readonly IUnityContainer _unityContainer;

        public TaskQueuePoolManager(IUnityContainer unityContainer, ITaskV2Service taskService)
        {
            _taskService = taskService;
            _unityContainer = unityContainer;
        }

        public void UpdateTaskQueues()
        {
            Console.WriteLine($"UpdateTaskQueues. Task queues count is {this._taskQueueManagers.Count}");
            foreach(var queueManager in this._taskQueueManagers)
            {
                Console.WriteLine($"\tQueue for user {queueManager.UserId}. Queue size is {queueManager.GetQueueSize()}");
            }

            // Get all pending tasks
            var tasks = this._taskService.GetPendingTasks(TaskTypes);
            // Group tasks by UserId
            var tasksGroupedByUserId = tasks.GroupBy(t => t.UserId);

            // For each task group add tasks to appropriate queue
            foreach(var taskGroup in tasksGroupedByUserId)
            {
                // Sort group tasks by creation date
                var userTasksSortedByDate = taskGroup.OrderBy(t => t.CreatedAt);

                // Look for user queue
                var taskQueueManager = this._taskQueueManagers.SingleOrDefault(m => m.UserId == taskGroup.Key);

                // If user queue exists - add tasks to queue
                if (taskQueueManager != null)
                {
                   taskQueueManager.AddToQueue(userTasksSortedByDate);
                }
                // If user queue doesnt exis - create queue, add tasks to queue and run queue
                else
                {
                    var handlerManager = this._unityContainer.Resolve<ITaskHandlerManager>();
                    var taskService = this._unityContainer.Resolve<ITaskV2Service>();
                    var userTaskQueueManager = new UserTaskQueueManager(new TaskQueue(), handlerManager, taskService, taskGroup.Key);
                    this._taskQueueManagers.Add(userTaskQueueManager);
                    userTaskQueueManager.AddToQueue(userTasksSortedByDate);
                    userTaskQueueManager.RunQueue();
                }
            }

            // Remove empty queues from queue list
            this._taskQueueManagers.RemoveAll(m => m.GetQueueSize() == 0);
        }
    }
}
