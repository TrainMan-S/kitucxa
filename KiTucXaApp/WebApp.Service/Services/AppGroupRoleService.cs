using WebApp.Data.Infrastructure;
using WebApp.Data.Repositories;
using WebApp.Model.Models;

namespace WebApp.Service.Services
{
    public interface IAppGroupRoleService
    {
        AppGroupRole AddGroupRole(AppGroupRole groupRole);

        void DeleteGroupRole(int groupId, string roleId);
        void DeleteGroupRoleByGroup(int groupId);
        void DeleteGroupRoleByRole(string roleId);

        void SaveChanges();
    }

    public class AppGroupRoleService : IAppGroupRoleService
    {
        private IUnitOfWork _unitOfWork;
        private IAppGroupRoleRepository _appGroupRoleRepository;

        public AppGroupRoleService(IUnitOfWork unitOfWork, IAppGroupRoleRepository appGroupRoleRepository)
        {
            this._unitOfWork = unitOfWork;
            this._appGroupRoleRepository = appGroupRoleRepository;
        }

        // *********************************
        // *********************************
        // *********************************

        public AppGroupRole AddGroupRole(AppGroupRole groupRole)
        {
            return _appGroupRoleRepository.Add(groupRole);
        }


        public void DeleteGroupRole(int groupId, string roleId)
        {
            _appGroupRoleRepository.DeleteMulti(m => m.GroupId == groupId && m.RoleId == roleId);
        }
        public void DeleteGroupRoleByGroup(int groupId)
        {
            _appGroupRoleRepository.DeleteMulti(m => m.GroupId == groupId);
        }
        public void DeleteGroupRoleByRole(string roleId)
        {
            _appGroupRoleRepository.DeleteMulti(m => m.RoleId == roleId);
        }


        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }
    }
}
