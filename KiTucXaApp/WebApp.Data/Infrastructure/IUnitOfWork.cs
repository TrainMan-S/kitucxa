namespace WebApp.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
