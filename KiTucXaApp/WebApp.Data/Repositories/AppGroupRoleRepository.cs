using WebApp.Data.Infrastructure;
using WebApp.Model.Models;

namespace WebApp.Data.Repositories
{
    public interface IAppGroupRoleRepository : IRepository<AppGroupRole>
    {

    }

    public class AppGroupRoleRepository : RepositoryBase<AppGroupRole>, IAppGroupRoleRepository
    {
        public AppGroupRoleRepository(IDbFactory dbFactory) : base(dbFactory) { }

        // *********************************
        // *********************************
        // *********************************

    }
}
