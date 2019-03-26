using WebApp.Data.Infrastructure;
using WebApp.Model.Models;

namespace WebApp.Data.Repositories
{
    public interface IAppGroupRepository : IRepository<AppGroup>
    {

    }

    public class AppGroupRepository : RepositoryBase<AppGroup>, IAppGroupRepository
    {
        public AppGroupRepository(IDbFactory dbFactory) : base(dbFactory) { }

        // *********************************
        // *********************************
        // *********************************

    }
}
