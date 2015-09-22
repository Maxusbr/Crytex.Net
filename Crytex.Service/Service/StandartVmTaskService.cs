using System;
using System.Collections.Generic;
using System.Linq;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using Crytex.Service.IService;

namespace Crytex.Service.Service
{
    public class StandartVmTaskService : IStandartVmTaskService
    {
        public StandartVmTaskService(IStandartVmTaskRepository standartVmTaskRepository, IUnitOfWork unitOfWork)
        {
            _standartVmTaskRepository = standartVmTaskRepository;
            _unitOfWork = unitOfWork;
        }

        IStandartVmTaskRepository _standartVmTaskRepository { get; }
        IUnitOfWork _unitOfWork { get; set; }

        public List<StandartVmTask> GetPageByVmId(int pageSize, int pageIndex, DateTime? dateFrom, DateTime? dateTo, Guid vmId)
        {
            var tasks = _standartVmTaskRepository.GetPage(new Page(pageIndex, pageSize),
                x => (x.VmId == vmId) &&
                (!dateFrom.HasValue || !dateTo.HasValue || x.CreatedDate > dateFrom.Value && x.CreatedDate < dateTo.Value),
                x => x.CreatedDate).ToList();
            return tasks;
        }

        public List<StandartVmTask> GetPageByUserId(int pageSize, int pageIndex, DateTime? dateFrom, DateTime? dateTo, string userId)
        {
            var tasks = _standartVmTaskRepository.GetPage(new Page(pageIndex, pageSize),
                x => (x.UserId == userId) &&
                (!dateFrom.HasValue || !dateTo.HasValue || x.CreatedDate > dateFrom.Value && x.CreatedDate < dateTo.Value),
                x => x.CreatedDate).ToList();
            return tasks;
        }

        public StandartVmTask GetTaskById(int id)
        {
            return _standartVmTaskRepository.GetById(id);
        }

        public StandartVmTask Create(Guid vmId, TypeStandartVmTask taskType, TypeVirtualization virtualization, string userId)
        {
            var task = new StandartVmTask()
            {
                VmId = vmId,
                TaskType = taskType,
                Virtualization = virtualization,
                UserId = userId,
                CreatedDate = DateTime.UtcNow
            };

            _standartVmTaskRepository.Add(task);
            _unitOfWork.Commit();
            return task;
        }

        public void Delete(int id)
        {
            _standartVmTaskRepository.Delete(x => x.Id == id);
            _unitOfWork.Commit();
        }

        public bool IsOwnerVm(Guid vmId, string userId)
        {
            var vm = _standartVmTaskRepository.GetById(vmId);
            return vm != null && vm.UserId == userId;
        }
    }
}