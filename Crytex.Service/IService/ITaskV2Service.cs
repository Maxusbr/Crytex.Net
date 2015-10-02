using System;
using System.Collections.Generic;
using Crytex.Service.Model;
using Crytex.Model.Models;
using PagedList;

namespace Crytex.Service.IService
{
    public interface ITaskV2Service
    {
        IPagedList<TaskV2> GetPageTasks(int pageNumber, int pageSize, TypeTask typeTask);
        TaskV2 GetTaskById(Guid id);
        TaskV2 CreateTask(TaskV2 createTask, string option);
        TaskV2 CreateTask<T>(TaskV2 createTask, T options) where T: BaseOptions;
        void UpdateTask(TaskV2 updateTask);
        void RemoveTask(Guid id);
    }
}
