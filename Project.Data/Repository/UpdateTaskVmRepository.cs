using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Data.Infrastructure;
using Project.Data.IRepository;
using Project.Model.Models;

namespace Project.Data.Repository
{
   public  class UpdateTaskVmRepository : RepositoryBase<UpdateVmTask>, IUpdateVmTaskRepository
    {
        public UpdateTaskVmRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
