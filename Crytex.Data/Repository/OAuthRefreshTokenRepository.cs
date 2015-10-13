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
    public class OAuthRefreshTokenRepository : RepositoryBase<OAuthRefreshToken>, IOAuthRefreshTokenRepository
    {
        public OAuthRefreshTokenRepository(IDatabaseFactory databaseFactory)
                : base(databaseFactory)
        { }
    }
}
