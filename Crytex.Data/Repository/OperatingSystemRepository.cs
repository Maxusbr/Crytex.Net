using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OperatingSystem = Crytex.Model.Models.OperatingSystem;

namespace Crytex.Data.Repository
{
    public class OperatingSystemRepository : RepositoryBase<OperatingSystem>, IOperatingSystemRepository
    {
        public OperatingSystemRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
