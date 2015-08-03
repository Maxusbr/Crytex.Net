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

namespace Project.Service.Service
{
    public class TaskVmBackGroundService : ITaskVmBackGroundService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISender _sender;
        private readonly ICreateVmTaskRepository _icreateVmTaskRepository;

        public TaskVmBackGroundService( IUnitOfWork unitOfWork, ISender sender, ICreateVmTaskRepository icreateVmTaskRepository)
        {
      
            _unitOfWork = unitOfWork;
            _sender = sender;
            _icreateVmTaskRepository = icreateVmTaskRepository;
        }

        public void CreateVm(CreateVmOption createVmOption)
        {
            var task = new CreateVmTask();
            task.Name = createVmOption.Name;
            task.Cpu = createVmOption.Cpu;
            task.Hdd = createVmOption.Hdd;
            task.Ram = createVmOption.Ram;
            task.StatusTask = StatusTask.Pending;

            _icreateVmTaskRepository.Add(task);
            _unitOfWork.Commit();

            _sender.SendCommand(task);



        }

        public void RemoveVm(RemoveVmOption removeVm)
        {
            throw new NotImplementedException();
        }

        public void UpdateVmOption(UpdateVmOption updateVmOption)
        {
            throw new NotImplementedException();
        }
    }
}
