using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using Crytex.Model.Models.Biling;

namespace Crytex.Data.Repository
{
    public class BoughtPhysicalServerRepository: RepositoryBase<BoughtPhysicalServer>, IBoughtPhysicalServerRepository
    {
        public BoughtPhysicalServerRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
