using System;
using Crytex.ExecutorTask.TaskHandler;
using Crytex.Model.Models;
using System.Collections.Generic;
using Crytex.Service.IService;

namespace Crytex.ExecutorTask
{
    internal class UserTaskQueueManager
    {
        private readonly ITaskHandlerManager _handlerManager;
        private readonly TaskQueue _queue;
        private readonly ITaskV2Service _taskService;

        public string UserId { get; private set; }

        public UserTaskQueueManager(TaskQueue queue, ITaskHandlerManager handlerManager, ITaskV2Service taskService, string userId)
        {
            this._queue = queue;
            this._handlerManager = handlerManager;
            this._taskService = taskService;
            this.UserId = userId;
        }

        internal void RunQueue()
        {
            this._queue.ExecuteAsync();
        }

        internal void AddToQueue(IEnumerable<TaskV2> tasks)
        {
            var handlers = this._handlerManager.GetTaskHandlers(tasks);
            foreach(var handler in handlers)
            {
                this._taskService.UpdateTaskStatus(handler.TaskEntity.Id, StatusTask.Queued);
                this._queue.AddToQueue(handler);
            }
        }

        internal int GetQueueSize()
        {
            return this._queue.GetQueueSize();
        }
    }
}
