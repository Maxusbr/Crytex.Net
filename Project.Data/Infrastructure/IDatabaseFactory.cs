using System;

namespace Project.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        ApplicationDbContext Get();
    }
}
