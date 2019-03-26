using WebApp.Data.Infrastructure;
using WebApp.Model.Models;

namespace WebApp.Data.Repositories
{
    public interface IAppRegionRepository : IRepository<AppRegion>
    {

    }

    public class AppRegionRepository : RepositoryBase<AppRegion>, IAppRegionRepository
    {
        public AppRegionRepository(IDbFactory dbFactory) : base(dbFactory) { }

        // *********************************
        // *********************************
        // *********************************

    }
}
