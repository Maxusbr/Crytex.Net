using System;
using System.Collections.Generic;
using Crytex.Service.Model;
using Crytex.Model.Models;
using PagedList;

namespace Crytex.Service.IService
{
    public interface ITaskVmService
    {
        CreateVmTask CreateVm(CreateVmTask createVmOption);
        void RemoveVm(RemoveVmOption removeVm);
        void UpdateVmOption(UpdateVmOption updateVmOption);

        IPagedList<CreateVmTask> GetCreateVmTasksForUser(int pageNumber, int pageSize,string userId, DateTime? from, DateTime? to);

        IPagedList<UpdateVmTask> GetUpdateVmTasksForUser(int pageNumber, int pageSize, string userId);

        IEnumerable<CreateVmTask> GetPendingCreateTasks();

        IEnumerable<UpdateVmTask> GetPendingUpdateTasks();

        IEnumerable<StandartVmTask> GetPendingStandartTasks();

        void UpdateTaskStatus<T>(int id, StatusTask newStatu, string message) where T : BaseTask;

        CreateVmTask GetCreateVmTaskById(int id);

        void DeleteCreateVmTaskById(int id);

        UpdateVmTask CreateUpdateVmTask(UpdateVmTask task, string userId = null);

        UpdateVmTask GetUpdateTaskById(int id, string userId = null);

    }
}
