using PagedList;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.IService;
using System;
using Crytex.Service.Extension;
using System.Linq.Expressions;
using Crytex.Service.Model;
using System.Collections.Generic;
using System.Linq;

namespace Crytex.Service.Service
{
    public class TaskV2Service : ITaskV2Service
    {
        private ITaskV2Repository _taskV2Repo;
        private IUnitOfWork _unitOfWork;
        private readonly IUserVmService _userVmService;
        private readonly IOperatingSystemsService _operatingSystemService;

        public TaskV2Service(ITaskV2Repository taskV2Repo, IUserVmService userVmService, IUnitOfWork unitOfWork)
        {
            this._taskV2Repo = taskV2Repo;
            this._userVmService = userVmService;
            this._unitOfWork = unitOfWork;
        }

        public virtual TaskV2 GetTaskById(Guid id)
        {
            var task = this._taskV2Repo.GetById(id);
            if (task == null)
            {
                throw new InvalidIdentifierException(string.Format("Task with id={0} doesnt exist.", id));
            }

            return task;
        }

        public TaskV2 CreateTask<T>(TaskV2 task, T options) where T : BaseOptions
        {
            task.SaveOptions(options);

            return this.CreateTask(task, task.Options); 
        }

        public TaskV2 CreateTask(TaskV2 task, string option)
        {
            task.Id = Guid.NewGuid();
            task.Options = option;

            task.CreatedAt = DateTime.UtcNow;
            task.StatusTask = StatusTask.Pending;

            this.ValidateTaskOptions(task);
            this.CreateOrRemoveTaskRelatedDbEntities(task);

            this._taskV2Repo.Add(task);
            this._unitOfWork.Commit();

            return task;
        }

        private void ValidateTaskOptions(TaskV2 task)
        {
            if (task.TypeTask == TypeTask.UpdateVm)
            {
                var updateOptions = task.GetOptions<UpdateVmOptions>();
                var vm = this._userVmService.GetVmById(updateOptions.VmId);
                var hardwareConf = vm.GetVmHardwareConfiguration();

                hardwareConf.RamMB = updateOptions.Ram;
                hardwareConf.Cpu = updateOptions.Cpu;
                hardwareConf.HardDriveSizeGB = updateOptions.HddGB;

                this._userVmService.CheckOsHardwareMinRequirements(hardwareConf, vm.OperatingSystemId);
            }
        }

        private void CreateOrRemoveTaskRelatedDbEntities(TaskV2 task)
        {
            if (task.TypeTask == TypeTask.CreateVm)
            {
                var createOptions = task.GetOptions<CreateVmOptions>();
                var newVm = new UserVm
                {
                    Id = Guid.NewGuid(),
                    CoreCount = createOptions.Cpu,
                    HardDriveSize = createOptions.HddGB,
                    RamCount = createOptions.Ram,
                    VirtualizationType = task.Virtualization,
                    Name = createOptions.Name,
                    OperatingSystemId = createOptions.OperatingSystemId,
                    Status = StatusVM.Creating,
                    UserId = task.UserId
                };
                createOptions.UserVmId = newVm.Id;
                task.SaveOptions(createOptions);

                this._userVmService.CreateVm(newVm);
            }
        }

        public IPagedList<TaskV2> GetPageTasks(int pageNumber, int pageSize, TaskV2SearchParams searchParams = null)
        {
            var page = new PageInfo(pageNumber, pageSize);

            Expression<Func<TaskV2, bool>> where = x => true;
            
            if (searchParams != null)
            {
                if (searchParams.TypeTask != null)
                {
                    where = where.And(x => x.TypeTask == searchParams.TypeTask);
                }

                if (searchParams.Virtualization != null)
                {
                    where = where.And(x => x.Virtualization == searchParams.Virtualization);
                }

                if (searchParams.ResourceId != null)
                {
                    where = where.And(x => x.ResourceId == searchParams.ResourceId);
                }

                if (searchParams.StatusTasks.Length != 0)
                {
                    where = where.And(x => searchParams.StatusTasks.Contains(x.StatusTask));
                    //x => x.StatusTask == StatusTask.Start|| x.StatusTask == StatusTask.Pending||x.StatusTask ==StatusTask.Processing
                }

                if (searchParams.UserId != null)
                {
                    where = where.And(x => x.UserId == searchParams.UserId);
                }

                if (searchParams.TypeDate != null)
                {
 
                    if (searchParams.TypeDate == TypeDate.StartedAt)
                    {
                        where = where.And(x => x.StartedAt >= searchParams.DateFrom && x.StartedAt <= searchParams.DateTo);
                    }                   
                }
            }

            var list = this._taskV2Repo.GetPage(page, where, x => x.CompletedAt);
            return list;
        }

        public void UpdateTask(TaskV2 updateTask)
        {
            var task = this.GetTaskById(updateTask.Id);

            task.ResourceId = updateTask.ResourceId;
            task.Options = updateTask.Options;

            this._taskV2Repo.Update(task);
            this._unitOfWork.Commit();
        }

        public void RemoveTask(Guid id)
        {
            var task = this.GetTaskById(id);

            this._taskV2Repo.Delete(task);
            this._unitOfWork.Commit();
        }

        public IEnumerable<TaskV2> GetPendingTasks(IEnumerable<TypeTask> taskTypes = null)
        {
            Expression<Func<TaskV2, bool>> where = t => t.StatusTask == StatusTask.Pending;

            if (taskTypes != null && taskTypes.Any())
            {
                Expression<Func<TaskV2, bool>> taskTypeWhere = x => false;
                foreach (var taskType in taskTypes)
                {
                    taskTypeWhere = taskTypeWhere.Or(x => x.TypeTask == taskType);
                }

                where = where.And(taskTypeWhere);
            }

            var tasks = this._taskV2Repo.GetMany(where);

            return tasks;
        }

        public void UpdateTaskStatus(Guid id, StatusTask status, DateTime? date = null, string errorMessage = null)
        {
            var task = this.GetTaskById(id);

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

        public void StopAllUserMachines(string userId)
        {
            var userVms = this._userVmService.GetAllVmsByUserId(userId);
            if (userVms.Count() > 0)
            {
                foreach (var vm in userVms)
                {
                    var task = new TaskV2
                    {
                        Id = Guid.NewGuid(),
                        ResourceType = ResourceType.SubscriptionVm,
                        ResourceId = vm.Id,
                        TypeTask = TypeTask.ChangeStatus,
                        Virtualization = vm.VirtualizationType,
                        StatusTask = StatusTask.Pending,
                        CreatedAt = DateTime.UtcNow,
                        UserId = vm.UserId
                    };

                    task.SaveOptions(new ChangeStatusOptions
                    {
                        TypeChangeStatus = TypeChangeStatus.Stop,
                        VmId = vm.Id
                    });

                    _taskV2Repo.Add(task);
                }
                this._unitOfWork.Commit();
            }
            
        }
    }
}


