using PagedList;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.IService;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace Crytex.Service.Service
{
    public class TaskV2Service : ITaskV2Service
    {
        private ITaskV2Repository _taskV2Repo;
        private IUnitOfWork _unitOfWork;

        public TaskV2Service(ITaskV2Repository taskV2Repo, IUnitOfWork unitOfWork)
        {
            this._taskV2Repo = taskV2Repo;
            this._unitOfWork = unitOfWork;
        }

        public TaskV2 GetTaskById(Guid id)
        {
            var vm = this._taskV2Repo.GetById(id);
            if (vm == null)
            {
                throw new InvalidIdentifierException(string.Format("UserVm with id={0} doesnt exist.", id));
            }

            return vm;
        }

        public TaskV2 CreateTask<T>(TaskV2 createTask, T options) where T : BaseOptions
        {
            TaskV2 task = createTask;
            task.Id = Guid.NewGuid();
            task.SaveOptions(options);

            this._taskV2Repo.Add(task);
            this._unitOfWork.Commit();

            return task;
        }

        public TaskV2 CreateTask(TaskV2 createTask, string option)
        {
            TaskV2 task = createTask;
            task.Id = Guid.NewGuid();
            task.Options = option;

            this._taskV2Repo.Add(task);
            this._unitOfWork.Commit();

            return task;
        }


        public IPagedList<TaskV2> GetPageTasks(int pageNumber, int pageSize, TypeTask typeTask)
        {
            var page = new Page(pageNumber, pageSize);
            var list = this._taskV2Repo.GetPage(page, x => x.TypeTask == typeTask, x => x.Id);
            return list;
        }

        public void UpdateTask(TaskV2 updateTask)
        {
            var task = this._taskV2Repo.GetById(updateTask.Id);
            
            if (task == null)
            {
                throw new InvalidIdentifierException(string.Format("UserVm with Id = {0} doesnt exist.", updateTask.Id));
            }

            task.ResourceId = updateTask.ResourceId;
            task.StatusTask = StatusTask.End;
            
            if (updateTask.TypeTask == TypeTask.CreateVm || updateTask.TypeTask == TypeTask.UpdateVm)
            {
                task.SaveOptions(updateTask.GetOptions<ConfigVmOptions>());
            }
            else if(updateTask.TypeTask == TypeTask.ChangeStatus)
            {
                task.SaveOptions(updateTask.GetOptions<ChangeStatusOptions>());
            }
            else
            {
                return;
            }

            this._taskV2Repo.Update(task);
            this._unitOfWork.Commit();
        }

        public void RemoveTask(Guid id)
        {
            var task = this._taskV2Repo.GetById(id);

            if (task == null)
            {
                throw new InvalidIdentifierException(string.Format("Region with Id={0} doesn't exists", id));
            }

            this._taskV2Repo.Delete(task);
            this._unitOfWork.Commit();
        }

        public IEnumerable<TaskV2> GetPendingTasks()
        {
            var tasks = this._taskV2Repo.GetMany(t => t.StatusTask == StatusTask.Pending);

            return tasks;
        }

        public void UpdateTaskStatus(Guid id, StatusTask status, DateTime? date = null, string errorMessage = null)
        {
            var task = this._taskV2Repo.GetById(id);

            if (task == null)
            {
                throw new InvalidIdentifierException(string.Format("Task with Id={0} doesn't exists", id.ToString()));
            }

            task.StatusTask = status;
            if (errorMessage != null)
            {
                task.ErrorMessage = errorMessage;
            }
            if (status == StatusTask.Start && date != null)
            {
                task.StartedAt = date.Value;
            }
            if((status == StatusTask.End || status == StatusTask.EndWithErrors) && date != null)
            {
                task.CompletedAt = date.Value;
            }

            this._taskV2Repo.Update(task);
            this._unitOfWork.Commit();
        }
    }
}
