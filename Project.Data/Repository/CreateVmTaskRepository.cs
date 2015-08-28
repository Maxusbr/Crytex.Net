using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Data.Infrastructure;
using Project.Data.IRepository;
using Project.Model.Models;
using PagedList;

namespace Project.Data.Repository
{
    public class CreateVmTaskRepository : RepositoryBase<CreateVmTask>, ICreateVmTaskRepository
    {
        public CreateVmTaskRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
