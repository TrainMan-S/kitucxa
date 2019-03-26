namespace WebApp.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        WebAppDbContext dbContext;

        public WebAppDbContext Init()
        {
            return dbContext ?? (dbContext = new WebAppDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
            }
        }
    }
}
