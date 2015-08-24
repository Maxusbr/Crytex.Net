using Project.Data.Infrastructure;
using Project.Data.IRepository;
using Project.Model.Models;

namespace Project.Data.Repository
{
    public class LogRepository:RepositoryBase<LogEntry>, ILogRepository
    {
        public LogRepository(IDatabaseFactory databaseFactory)
                : base(databaseFactory) { }
    }
}