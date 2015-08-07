using Project.Data.Infrastructure;
using Project.Data.IRepository;
using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Repository
{
    public class HelpDeskRequestCommentRepository : RepositoryBase<HelpDeskRequestComment>, IHelpDeskRequestCommentRepository
    {
        public HelpDeskRequestCommentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
