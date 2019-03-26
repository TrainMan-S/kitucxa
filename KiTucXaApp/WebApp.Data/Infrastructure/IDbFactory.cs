using System;

namespace WebApp.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        WebAppDbContext Init();
    }
}
