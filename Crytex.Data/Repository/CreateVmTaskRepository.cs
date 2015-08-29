using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using PagedList;

namespace Crytex.Data.Repository
{
    public class CreateVmTaskRepository : RepositoryBase<CreateVmTask>, ICreateVmTaskRepository
    {
        public CreateVmTaskRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
