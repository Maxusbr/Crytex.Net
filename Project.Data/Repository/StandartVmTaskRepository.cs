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
    public class StandartVmTaskRepository : RepositoryBase<StandartVmTask>,IStandartVmTaskRepository
    {
        public StandartVmTaskRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
