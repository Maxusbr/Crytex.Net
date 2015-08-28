using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Model.Models;
using Project.Data.Infrastructure;
using PagedList;

namespace Project.Data.IRepository
{
    public interface ICreateVmTaskRepository : IRepository<CreateVmTask>
    {
    }
}
