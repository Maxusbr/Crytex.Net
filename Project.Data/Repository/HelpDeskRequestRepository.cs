using Project.Data.Infrastructure;
using Project.Data.IRepository;
using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project.Data.Repository
{
    public class HelpDeskRequestRepository : RepositoryBase<HelpDeskRequest>, IHelpDeskRequestRepository
    {
        public HelpDeskRequestRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
