using System.Linq;
using WebApp.Data.Infrastructure;
using WebApp.Model.Models;

namespace WebApp.Data.Repositories
{
    public interface IAppUserRepository : IRepository<AppUser>
    {
        AppUser GetUserByName(string username);
        AppUser GetUserById(string id);

        IQueryable<AppUser> GetUsersByRoleId(string roleId);
        IQueryable<AppUser> GetUsersActiveByRoleId(string roleId);
    }


    public class AppUserRepository : RepositoryBase<AppUser>, IAppUserRepository
    {
        public AppUserRepository(IDbFactory dbFactory) : base(dbFactory) { }

        // *********************************
        // *********************************
        // *********************************

        public AppUser GetUserByName(string username)
        {
            return DbContext.Users.SingleOrDefault(m => m.UserName == username);
        }

        public AppUser GetUserById(string id)
        {
            return DbContext.Users.SingleOrDefault(m => m.Id == id);
        }

        public IQueryable<AppUser> GetUsersByRoleId(string roleId)
        {
            return (from r in DbContext.AppRoles
                    join gr in DbContext.AppGroupRoles on r.Id equals gr.RoleId
                    join u in DbContext.Users on gr.GroupId equals u.GroupId
                    where r.Id == roleId
                    select u);
        }

        public IQueryable<AppUser> GetUsersActiveByRoleId(string roleId)
        {
            return (from r in DbContext.AppRoles
                    join gr in DbContext.AppGroupRoles on r.Id equals gr.RoleId
                    join u in DbContext.Users on gr.GroupId equals u.GroupId
                    where r.Id == roleId && u.IsActived
                    select u);
        }

    }
}
