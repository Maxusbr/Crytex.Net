using System.Data.Entity;

namespace Project.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;
        private ApplicationDbContext _dataContext;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            this._databaseFactory = databaseFactory;
        }

        protected ApplicationDbContext DataContext
        {
            get { return _dataContext ?? (_dataContext = _databaseFactory.Get()); }
        }

        public void Commit()
        {
            DataContext.Commit();
        }


        public void Rollback()
        {
            foreach (var dbEntityEntry in _dataContext.ChangeTracker.Entries())
            {
                switch (dbEntityEntry.State)
                {
                    case EntityState.Added:
                        dbEntityEntry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                    case EntityState.Modified:
                        dbEntityEntry.Reload();
                        break;
                }
            }

        }
    }
}
