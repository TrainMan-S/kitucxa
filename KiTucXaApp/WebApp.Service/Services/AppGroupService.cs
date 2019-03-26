using System.Linq;
using WebApp.Data.Infrastructure;
using WebApp.Data.Repositories;
using WebApp.Model.Models;

namespace WebApp.Service.Services
{
    public interface IAppGroupService
    {
        IQueryable<AppGroup> GetGroups();

        AppGroup GetGroupById(int id);
        AppGroup AddGroup(AppGroup group);
        AppGroup UpdateGroup(AppGroup group);

        bool CheckGroupExits(int id);

        void DeleteGroup(int id);
        void SaveChanges();
    }
    public class AppGroupService : IAppGroupService
    {
        private IAppGroupRepository _appGroupRepository;
        private IAppGroupRoleRepository _appGroupRoleRepository;
        private IUnitOfWork _unitOfWork;

        public AppGroupService(
            IAppGroupRepository applicationGroupRepository,
            IAppGroupRoleRepository aplicationGroupRoleRepository,
            IUnitOfWork unitOfWork)
        {
            this._appGroupRepository = applicationGroupRepository;
            this._appGroupRoleRepository = aplicationGroupRoleRepository;
            this._unitOfWork = unitOfWork;
        }

        // *********************************
        // *********************************
        // *********************************

        public IQueryable<AppGroup> GetGroups()
        {
            return _appGroupRepository.GetAll().OrderBy(m => m.SortOrder);
        }


        public AppGroup GetGroupById(int id)
        {
            return _appGroupRepository.GetSingleById(id);
        }
        public AppGroup AddGroup(AppGroup group)
        {
            return _appGroupRepository.Add(group);
        }
        public AppGroup UpdateGroup(AppGroup group)
        {
            return _appGroupRepository.Update(group);
        }


        public bool CheckGroupExits(int id)
        {
            return _appGroupRepository.CheckContains(m => m.GroupId == id);
        }


        public void DeleteGroup(int id)
        {
            _appGroupRoleRepository.DeleteMulti(m => m.GroupId == id);
            _appGroupRepository.DeleteMulti(m => m.GroupId == id);
        }
        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

    }
}
