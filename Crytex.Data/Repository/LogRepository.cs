using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class LogRepository:RepositoryBase<LogEntry>, ILogRepository
    {
        public LogRepository(IDatabaseFactory databaseFactory)
                : base(databaseFactory) { }
    }
}