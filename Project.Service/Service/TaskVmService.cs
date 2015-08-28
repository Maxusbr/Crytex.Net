using System;
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
using Project.Model.Exceptions;
using PagedList;
using System.Linq.Expressions;

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
        private readonly IFileDescriptorRepository _fileDescriptorRepo;

        public TaskVmService(IUnitOfWork unitOfWork, ICreateVmTaskRepository createVmTaskRepository, IUpdateVmTaskRepository updateVmTaskRepository,
            IStandartVmTaskRepository standartVmTaskRepository, IUserVmRepository userVmRepository, IServerTemplateRepository serverTemplateRepository,
            IFileDescriptorRepository fileDescriptorRepo)
        {
            this._unitOfWork = unitOfWork;
            this._createVmTaskRepository = createVmTaskRepository;
            this._updateVmTaskRepository = updateVmTaskRepository;
            this._standartVmTaskRepository = standartVmTaskRepository;
            this._userVmRepository = userVmRepository;
            this._serverTemplateRepository = serverTemplateRepository;
            this._fileDescriptorRepo = fileDescriptorRepo;
        }

        public CreateVmTask CreateVm(CreateVmTask createVmTask)
        {
            // Validation block
            var template = this._serverTemplateRepository.GetById(createVmTask.ServerTemplateId);
            var existedNameTaskOrVm = this._userVmRepository.Get(m => m.Name == createVmTask.Name) as object ??
                this._createVmTaskRepository.Get(t => t.Name == createVmTask.Name) as object;

            if (existedNameTaskOrVm != null)
            {
                throw new ValidationException(string.Format("VM with name {0} already exists for this user.", createVmTask.Name));
            }
            if (template.MinCoreCount > createVmTask.Cpu)
            {
                throw new ValidationException(string.Format("VM's CoreCount cannot be less than it's template value in {0} template", template.Name));
            }
            if (template.MinRamCount > createVmTask.Ram)
            {
                throw new ValidationException(string.Format("VM's Ram cannot be less than it's template value in {0} template", template.Name));
            }
            if (template.MinHardDriveSize > createVmTask.Hdd)
            {
                throw new ValidationException(string.Format("VM's HDD size cannot be less than it's template value in {0} template", template.Name));
            }
            //End validation

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


        public IPagedList<CreateVmTask> GetCreateVmTasksForUser(int pageNumber, int pageSize, string userId, DateTime? from, DateTime? to)
        {
            var page = new Page(pageNumber, pageSize);
            var expression = this.BuildSearchExpression(userId, from, to);
            var tasks = this._createVmTaskRepository.GetPage(page, expression, x => x.CreationDate,
                x => x.ServerTemplate,
                x => x.ServerTemplate.ImageFileDescriptor);

            return tasks;
        }

        private Expression<Func<CreateVmTask, bool>> BuildSearchExpression(string userId, DateTime? from, DateTime? to)
        {
            if (from == null)
            {
                from = DateTime.MinValue;
            }
            if (to == null)
            {
                to = DateTime.MaxValue;
            }
            Expression<Func<CreateVmTask, bool>> exp = x => userId == null ? true : x.UserId == userId && x.CreationDate >= from && x.CreationDate <= to;

            return exp;
        }


        public CreateVmTask GetCreateVmTaskById(int id)
        {
            var task = this._createVmTaskRepository.Get(x => x.Id == id,
                x => x.ServerTemplate,
                x => x.ServerTemplate.ImageFileDescriptor);

            if (task == null)
            {
                throw new InvalidIdentifierException(string.Format("CreateVmTask with id={0] doesnt exist", id));
            }

            return task;
        }


        public void DeleteCreateVmTaskById(int id)
        {
            var task = this._createVmTaskRepository.GetById(id);
            if (task == null)
            {
                throw new InvalidIdentifierException(string.Format("CreateVmTask with id={0] doesnt exist", id));
            }
            if (task.StatusTask == StatusTask.Processing || task.StatusTask == StatusTask.Start)
            {
                throw new TaskOperationException(string.Format("Cannot delete task with status {0}", task.StatusTask));
            }

            this._createVmTaskRepository.Delete(task);
            this._unitOfWork.Commit();

            return;
        }
    }
}
