using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data
{
    internal class DbConfig : DbConfiguration
    {
        public DbConfig()
        {
            this.SetDefaultConnectionFactory(new System.Data.Entity.Infrastructure.SqlConnectionFactory());
            this.SetProviderServices("System.Data.SqlClient", System.Data.Entity.SqlServer.SqlProviderServices.Instance);
        }
    }
}
