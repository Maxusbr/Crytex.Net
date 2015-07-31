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
    public class TaskVmService : ITaskVmService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISender _sender;
        public TaskVmService( IUnitOfWork unitOfWork, ISender Sender)
        {
      
            _unitOfWork = unitOfWork;
            _sender = Sender;
        }
    }
}
