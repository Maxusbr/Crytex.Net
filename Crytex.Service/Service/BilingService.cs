using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Data.Infrastructure;
using Crytex.Service.IService;

namespace Crytex.Service.Service
{
    public class BilingService : IBilingService
    {
        private IUnitOfWork _unitOfWork;

        public BilingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


       
    }
}
