using System.Linq;
using WebApp.Data.Infrastructure;
using WebApp.Data.Repositories;
using WebApp.Model.Models;

namespace WebApp.Service.Services
{
    public interface IAppRoleService
    {
        IQueryable<AppRole> GetRoles();
        IQueryable<AppRole> GetRolesByUserId(string userId);
        IQueryable<AppRole> GetRolesByGroupId(int groupId);
        IQueryable<AppRole> GetRolesByRegionId(int regionId);

        AppRole GetRoleById(string id);
        AppRole AddRole(AppRole role);
        AppRole UpdateRole(AppRole role);

        bool CheckNameRole(string name);

        void DeleteRole(string id);
        void SaveChanges();
    }
    public class AppRoleService : IAppRoleService
    {
        private IAppRoleRepository _appRoleRepository;
        private IAppGroupRoleRepository _appGroupRoleRepository;
        private IAppGroupRepository _appGroupRepository;
        private IUnitOfWork _unitOfWork;

        public AppRoleService(IAppRoleRepository appRoleRepository,
            IAppGroupRoleRepository appGroupRoleRepository,
            IAppGroupRepository appGroupRepository,
            IUnitOfWork unitOfWork)
        {
            this._appRoleRepository = appRoleRepository;
            this._appGroupRoleRepository = appGroupRoleRepository;
            this._appGroupRepository = appGroupRepository;
            this._unitOfWork = unitOfWork;
        }

        // *********************************
        // *********************************
        // *********************************

        public IQueryable<AppRole> GetRoles()
        {
            return _appRoleRepository.GetAll(new string[] { "AppRegion" }).OrderBy(m => m.RegionId);
        }
        public IQueryable<AppRole> GetRolesByUserId(string userId)
        {
            return _appRoleRepository.GetRolesByUserId(userId);
        }
        public IQueryable<AppRole> GetRolesByGroupId(int groupId)
        {
            return _appRoleRepository.GetRolesByGroupId(groupId);
        }
        public IQueryable<AppRole> GetRolesByRegionId(int regionId)
        {
            return _appRoleRepository.GetMulti(m => m.RegionId == regionId);
        }


        public AppRole GetRoleById(string idRole)
        {
            return _appRoleRepository.GetSingleById(idRole);
        }
        public AppRole AddRole(AppRole role)
        {
            return _appRoleRepository.Add(role);
        }
        public AppRole UpdateRole(AppRole role)
        {
            return _appRoleRepository.Update(role);
        }


        public bool CheckNameRole(string name)
        {
            return _appRoleRepository.CheckContains(m => m.Name == name);
        }

        public void DeleteRole(string id)
        {
            _appGroupRoleRepository.DeleteMulti(m => m.RoleId == id);
            _appRoleRepository.DeleteMulti(m => m.Id == id);
        }
        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }
    }
}
