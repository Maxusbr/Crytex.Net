﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Data.Infrastructure;
using Project.Data.IRepository;
using Project.Model.Models;
using Project.Service.IService;
using Project.Service.Model;
using Microsoft.Practices.Unity.Utility;

namespace Project.Service.Service
{
    public class TaskVmService : ITaskVmService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICreateVmTaskRepository _createVmTaskRepository;
        private readonly IUpdateVmTaskRepository _updateVmTaskRepository;
        private readonly IStandartVmTaskRepository _standartVmTaskRepository;
        private readonly IUserVmRepository _userVmRepository;
        private readonly IServerTemplateRepository _serverTemplateRepository;

        public TaskVmService(IUnitOfWork unitOfWork, ICreateVmTaskRepository createVmTaskRepository, IUpdateVmTaskRepository updateVmTaskRepository,
            IStandartVmTaskRepository standartVmTaskRepository, IUserVmRepository userVmRepository, IServerTemplateRepository serverTemplateRepository)
        {
            this._unitOfWork = unitOfWork;
            this._createVmTaskRepository = createVmTaskRepository;
            this._updateVmTaskRepository = updateVmTaskRepository;
            this._standartVmTaskRepository = standartVmTaskRepository;
            this._userVmRepository = userVmRepository;
            this._serverTemplateRepository = serverTemplateRepository;
        }

        public CreateVmTask CreateVm(CreateVmTask createVmTask)
        {
            var template = this._serverTemplateRepository.GetById(createVmTask.ServerTemplateId);
            
            // !!validate!!

            createVmTask.StatusTask = StatusTask.Pending;
            createVmTask.CreationDate = DateTime.UtcNow;

            _createVmTaskRepository.Add(createVmTask);
            _unitOfWork.Commit();

            return createVmTask;
        }

        public void RemoveVm(RemoveVmOption removeVm)
        {
            throw new NotImplementedException();
        }

        public void UpdateVmOption(UpdateVmOption updateVmOption)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<CreateVmTask> GetPendingCreateTasks()
        {
            var tasks = this.GetPendingTasks<CreateVmTask>(this._createVmTaskRepository);
            return tasks;
        }

        public IEnumerable<UpdateVmTask> GetPendingUpdateTasks()
        {
            var tasks = this.GetPendingTasks<UpdateVmTask>(this._updateVmTaskRepository);
            return tasks;
        }

        public IEnumerable<StandartVmTask> GetPendingStandartTasks()
        {
            var tasks = this.GetPendingTasks<StandartVmTask>(this._standartVmTaskRepository);
            return tasks;
        }

        private IEnumerable<T> GetPendingTasks<T>(IRepository<T> repo) where T : BaseTask
        {
            return repo.GetMany(task => task.StatusTask == StatusTask.Pending);
        }

        private IRepository<T> GerRepoByTaskType<T>() where T : BaseTask
        {
            var type = typeof(T);
            if (type == typeof(StandartVmTask))
            {
                return (IRepository<T>)this._standartVmTaskRepository;
            }
            if (type == typeof(UpdateVmTask))
            {
                return (IRepository<T>)this._updateVmTaskRepository;
            } 
            if (type == typeof(CreateVmTask))
            {
                return (IRepository<T>)this._createVmTaskRepository;
            }

            throw new Exception("This task type isn't mapped on any repo");
        }

        public void UpdateTaskStatus<T>(int id, StatusTask newStatus) where T : BaseTask
        {
            var repo = this.GerRepoByTaskType<T>();
            var taskToUpdate = repo.GetById(id);
            taskToUpdate.StatusTask = newStatus;
            repo.Update(taskToUpdate);
            this._unitOfWork.Commit();
        }
    }
}
