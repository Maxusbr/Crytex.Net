using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;

namespace Crytex.Data.Repository
{
    public class VmBackupRepository : RepositoryBase<VmBackup>, IVmBackupRepository
    {
        public VmBackupRepository(IDatabaseFactory dbFactory) : base(dbFactory) { }
    }
}
