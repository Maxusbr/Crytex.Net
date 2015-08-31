using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class FileDescriptorRepository : RepositoryBase<FileDescriptor>, IFileDescriptorRepository
    {
        public FileDescriptorRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
