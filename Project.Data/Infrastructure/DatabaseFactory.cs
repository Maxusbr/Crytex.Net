namespace Project.Data.Infrastructure
{
public class DatabaseFactory : Disposable, IDatabaseFactory
{
    private ApplicationDbContext dataContext;
    public ApplicationDbContext Get()
    {
        return dataContext ?? (dataContext = new ApplicationDbContext());
    }
    protected override void DisposeCore()
    {
        if (dataContext != null)
            dataContext.Dispose();
    }
}
}
