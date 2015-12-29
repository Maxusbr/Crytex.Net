using System;
using System.Collections.Generic;
using Crytex.Service.Model;
using Crytex.Model.Models;
using PagedList;

namespace Crytex.Service.IService
{
    public interface ITaskV2Service
    {
        IPagedList<TaskV2> GetPageTasks(int pageNumber, int pageSize, TaskV2SearchParams searchParams = null);
        TaskV2 GetTaskById(Guid id);

        /// <summary>
        /// Создаёт новую задачу в системе и, если необходимо, связанные с ней сущности в базе данных
        /// </summary>
        TaskV2 CreateTask(TaskV2 createTask, string option);

        /// <summary>
        /// Создаёт новую задачу в системе и, если необходимо, связанные с ней сущности в базе данных
        /// </summary>
        TaskV2 CreateTask<T>(TaskV2 task, T options) where T: BaseOptions;
        void UpdateTask(TaskV2 updateTask);
        void RemoveTask(Guid id);
        IEnumerable<TaskV2> GetPendingTasks(TypeVirtualization virtualizationType);
        void UpdateTaskStatus(Guid id, StatusTask status, DateTime? date = null, string errorMessage = null);
        void StopAllUserMachines(string userId);
    }
}
