using System.Data.Entity;

namespace Crytex.Data
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
