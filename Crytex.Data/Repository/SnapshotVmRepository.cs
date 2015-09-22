using System;
using System.Linq;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using System.Linq.Expressions;
using PagedList;
using System.Data.Entity;

namespace Crytex.Data.Repository
{
   public  class SnapshotVmRepository : RepositoryBase<SnapshotVm>, ISnapshotVmRepository
    {
        public SnapshotVmRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

    }
}
