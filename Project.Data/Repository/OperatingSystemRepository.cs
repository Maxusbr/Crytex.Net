using Project.Data.Infrastructure;
using Project.Data.IRepository;
using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OperatingSystem = Project.Model.Models.OperatingSystem;

namespace Project.Data.Repository
{
    public class OperatingSystemRepository : RepositoryBase<OperatingSystem>, IOperatingSystemRepository
    {
        public OperatingSystemRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
