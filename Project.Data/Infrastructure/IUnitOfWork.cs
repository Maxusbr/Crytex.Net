
namespace Project.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
    }
}
