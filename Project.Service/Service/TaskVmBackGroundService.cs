using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Data.Infrastructure;
using Project.Data.IRepository;
using Project.Service.IService;

namespace Project.Service.Service
{
    public class TaskVmBackGroundService : ITaskVmBackGroundService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISender _sender;
        public TaskVmBackGroundService( IUnitOfWork unitOfWork, ISender Sender)
        {
      
            _unitOfWork = unitOfWork;
            _sender = Sender;
        }
    }
}
