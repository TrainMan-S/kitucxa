using System.Linq;
using WebApp.Data.Infrastructure;
using WebApp.Model.Models;

namespace WebApp.Data.Repositories
{
    public interface IAppRoleRepository : IRepository<AppRole>
    {
        IQueryable<AppRole> GetRolesByUserId(string userId);
        IQueryable<AppRole> GetRolesByGroupId(int groupId);
    }

    public class AppRoleRepository : RepositoryBase<AppRole>, IAppRoleRepository
    {
        public AppRoleRepository(IDbFactory dbFactory) : base(dbFactory) { }

        // *********************************
        // *********************************
        // *********************************

        public IQueryable<AppRole> GetRolesByUserId(string userId)
        {
            return (from u in DbContext.Users
                    join g in DbContext.AppGroups on u.GroupId equals g.GroupId
                    join gr in DbContext.AppGroupRoles on g.GroupId equals gr.GroupId
                    join r in DbContext.AppRoles on gr.RoleId equals r.Id
                    where u.Id == userId
                    select r);
        }

        public IQueryable<AppRole> GetRolesByGroupId(int groupId)
        {
            return (from g in DbContext.AppGroups
                    join gr in DbContext.AppGroupRoles on g.GroupId equals gr.GroupId
                    join r in DbContext.AppRoles on gr.RoleId equals r.Id
                    where g.GroupId == groupId
                    select r);
        }
    }
}
