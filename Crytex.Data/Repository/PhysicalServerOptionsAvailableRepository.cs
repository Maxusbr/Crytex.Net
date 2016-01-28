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
    public class PhysicalServerOptionsAvailableRepository: RepositoryBase<PhysicalServerOptionsAvailable>, IPhysicalServerOptionsAvailableRepository
    {
        public PhysicalServerOptionsAvailableRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
