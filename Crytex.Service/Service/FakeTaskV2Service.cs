using System;
using System.Collections.Generic;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Service.Model;
using PagedList;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;

namespace Crytex.Service.Service
{
    // This class is used for testing purpouses
    public class FakeTaskV2Service : ITaskV2Service
    {
        private ITaskV2Repository _taskV2Repo;
        private IUnitOfWork _unitOfWork;
        private IApplicationUserRepository _userRepo;
        private Random _rnd;

        public FakeTaskV2Service(ITaskV2Repository taskRepo, IUnitOfWork unitOfWork, IApplicationUserRepository userRepo)
        {
            this._taskV2Repo = taskRepo;
            this._unitOfWork = unitOfWork;
            this._userRepo = userRepo;
            this._rnd = new Random();
        }

        public TaskV2 CreateTask(TaskV2 createTask, string option)
        {
            var users = this._userRepo.GetAll();
            var randomUser = users[this._rnd.Next(0, 3)];
            var task = new TaskV2
            {
                Id = Guid.NewGuid(),
                Options = "",
                UserId = randomUser.Id,
                CreatedAt = DateTime.UtcNow,
                StatusTask = StatusTask.Pending,
                Virtualization = TypeVirtualization.HyperV,
                TypeTask = TypeTask.Test
            };
            this._taskV2Repo.Add(task);
            this._unitOfWork.Commit();

            return task;
        }

        public TaskV2 CreateTask<T>(TaskV2 task, T options) where T : BaseOptions
        {
            task.SaveOptions(options);
            return this.CreateTask(task, task.Options);
        }

        public IPagedList<TaskV2> GetPageTasks(int pageNumber, int pageSize, TaskV2SearchParams searchParams = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskV2> GetPendingTasks()
        {
            throw new NotImplementedException();
        }

        public TaskV2 GetTaskById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void RemoveTask(Guid id)
        {
            throw new NotImplementedException();
        }

        public void StopAllUserMachines(string userId)
        {
            throw new NotImplementedException();
        }

        public void UpdateTask(TaskV2 updateTask)
        {
            throw new NotImplementedException();
        }

        public void UpdateTaskStatus(Guid id, StatusTask status, DateTime? date = default(DateTime?), string errorMessage = null)
        {
            throw new NotImplementedException();
        }
    }
}
