using Project.Data.Infrastructure;
using Project.Data.IRepository;
using Project.Model.Models;

namespace Project.Data.Repository
{
    public class MessageRepository : RepositoryBase<Message>, IMessageRepository
    {
        public MessageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }




}
