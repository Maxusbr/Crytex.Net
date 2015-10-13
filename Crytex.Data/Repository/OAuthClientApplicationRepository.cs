using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Data.Repository
{
    public class OAuthClientApplicationRepository : RepositoryBase<OAuthClientApplication>, IOAuthClientApplicationRepository
    {
        public OAuthClientApplicationRepository(IDatabaseFactory databaseFactory)
                : base(databaseFactory)
        { }
    }
}
