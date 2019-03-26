using System;
using System.Linq;
using WebApp.Data.Infrastructure;
using WebApp.Data.Repositories;
using WebApp.Model.Models;

namespace WebApp.Service.Services
{
    public interface IAppUserService
    {
        AppUser GetUserById(string id);
        AppUser GetUserByName(string username);
        AppUser UpdateUser(AppUser user);

        IQueryable<AppUser> GetUsers();
        IQueryable<AppUser> GetUsersOfSystem();
        IQueryable<AppUser> GetUsersOfStudent();
        IQueryable<AppUser> GetUsersByRole(string roleId);
        IQueryable<AppUser> GetUsersActiveByRole(string roleId);
        IQueryable<AppUser> GetUsersByGroup(int groupId);
        IQueryable<AppUser> GetUsersActiveByGroup(int groupId);

        bool CheckUserActiveById(string id);
        bool CheckUserActiveByName(string username);
        bool CheckUsernameExist(string id, string username);
        bool CheckEmailExist(string id, string email);

        void SaveChanges();
    }
    public class AppUserService : IAppUserService
    {
        private IAppUserRepository _appUserRepository;
        private IUnitOfWork _unitOfWork;

        public AppUserService(IAppUserRepository appUserRepository, IUnitOfWork unitOfWork)
        {
            this._appUserRepository = appUserRepository;
            this._unitOfWork = unitOfWork;
        }

        // *********************************
        // *********************************
        // *********************************

        public IQueryable<AppUser> GetUsers()
        {
            return _appUserRepository.GetAll(new string[] { "AppGroup" }).OrderByDescending(m => m.CreatedDate);
        }
        public IQueryable<AppUser> GetUsersOfSystem()
        {
            return _appUserRepository.GetMulti(m => m.GroupId < 5, new string[] { "AppGroup" }).OrderByDescending(m => m.CreatedDate);
        }
        public IQueryable<AppUser> GetUsersOfStudent()
        {
            return _appUserRepository.GetMulti(m => m.GroupId == 5, new string[] { "Room" }).OrderByDescending(m => m.CreatedDate);
        }

        public IQueryable<AppUser> GetUsersByRole(string roleId)
        {
            return _appUserRepository.GetUsersByRoleId(roleId);
        }
        public IQueryable<AppUser> GetUsersActiveByRole(string roleId)
        {
            return _appUserRepository.GetUsersActiveByRoleId(roleId);
        }
        public IQueryable<AppUser> GetUsersByGroup(int groupId)
        {
            return _appUserRepository.GetMulti(m => m.GroupId == groupId, new string[] { "AppGroup" }).OrderByDescending(m => m.CreatedDate);
        }
        public IQueryable<AppUser> GetUsersActiveByGroup(int groupId)
        {
            return _appUserRepository.GetMulti(m => m.GroupId == groupId && m.IsActived);
        }

        public AppUser GetUserById(string id)
        {
            return _appUserRepository.GetSingleByCondition(m => m.Id == id, new string[] { "AppGroup" });
        }
        public AppUser GetUserByName(string username)
        {
            return _appUserRepository.GetSingleByCondition(m => m.UserName == username, new string[] { "AppGroup" });
        }
        public AppUser UpdateUser(AppUser user)
        {
            return _appUserRepository.Update(user);
        }


        public bool CheckUserActiveById(string id)
        {
            return _appUserRepository.CheckContains(m => m.Id == id && m.IsActived);
        }
        public bool CheckUserActiveByName(string name)
        {
            return _appUserRepository.CheckContains(m => m.UserName == name && m.IsActived);
        }
        public bool CheckUsernameExist(string id, string username)
        {
            if (string.IsNullOrEmpty(id))
            {
                return _appUserRepository.CheckContains(m => m.UserName.ToLower() == username.ToLower());
            }
            else
            {
                return _appUserRepository.CheckContains(m => m.UserName.ToLower() == username.ToLower() && m.Id != id);
            }
        }
        public bool CheckEmailExist(string id, string email)
        {
            if (string.IsNullOrEmpty(id))
            {
                return _appUserRepository.CheckContains(m => m.Email.ToLower() == email.ToLower());
            }
            else
            {
                return _appUserRepository.CheckContains(m => m.Email.ToLower() == email.ToLower() && m.Id != id);
            }
        }


        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

    }
}
