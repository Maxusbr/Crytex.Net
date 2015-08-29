using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class StandartVmTaskRepository : RepositoryBase<StandartVmTask>,IStandartVmTaskRepository
    {
        public StandartVmTaskRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
