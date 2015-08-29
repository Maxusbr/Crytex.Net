using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crytex.Data.Repository
{
    public class HelpDeskRequestRepository : RepositoryBase<HelpDeskRequest>, IHelpDeskRequestRepository
    {
        public HelpDeskRequestRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
