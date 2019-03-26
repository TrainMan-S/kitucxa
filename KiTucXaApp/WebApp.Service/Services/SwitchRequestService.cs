using System.Linq;
using WebApp.Data.Infrastructure;
using WebApp.Data.Repositories;
using WebApp.Model.Models;

namespace WebApp.Service.Services
{
    public interface ISwitchRequestService
    {
        IQueryable<SwitchRequest> GetSwitchRequests();
        IQueryable<SwitchRequest> GetSwitchRequestsOfStudentById(string userid);
        IQueryable<SwitchRequest> GetSwitchRequestsOfStudentByName(string username);

        SwitchRequest GetSwitchRequestById(string id);
        SwitchRequest AddSwitchRequest(SwitchRequest switchRequest);
        SwitchRequest UpdateSwitchRequest(SwitchRequest switchRequest);

        bool CheckSwitchRequestForStudent(string userid);

        void DeleteSwitchRequest(string id);
        void SaveChanges();
    }
    public class SwitchRequestService : ISwitchRequestService
    {
        private IUnitOfWork _unitOfWork;
        private ISwitchRequestRepository _switchRequestRepository;

        public SwitchRequestService(IUnitOfWork unitOfWork, ISwitchRequestRepository switchRequestRepository)
        {
            this._unitOfWork = unitOfWork;
            this._switchRequestRepository = switchRequestRepository;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        public IQueryable<SwitchRequest> GetSwitchRequests()
        {
            return _switchRequestRepository.GetAll(new string[] { "AppUser" }).OrderByDescending(m => m.CreatedDate);
        }
        public IQueryable<SwitchRequest> GetSwitchRequestsOfStudentById(string userid)
        {
            return _switchRequestRepository.GetMulti(m => m.Id == userid, new string[] { "AppUser" }).OrderByDescending(m => m.CreatedDate);
        }
        public IQueryable<SwitchRequest> GetSwitchRequestsOfStudentByName(string username)
        {
            return _switchRequestRepository.GetMulti(m => m.AppUser.Id == username, new string[] { "AppUser" }).OrderByDescending(m => m.CreatedDate);
        }


        public SwitchRequest GetSwitchRequestById(string id)
        {
            return _switchRequestRepository.GetSingleByCondition(m => m.SwitchRequestId == id, new string[] { "AppUser" });
        }
        public SwitchRequest AddSwitchRequest(SwitchRequest switchRequest)
        {
            return _switchRequestRepository.Add(switchRequest);
        }
        public SwitchRequest UpdateSwitchRequest(SwitchRequest switchRequest)
        {
            return _switchRequestRepository.Update(switchRequest);
        }


        public bool CheckSwitchRequestForStudent(string userid)
        {
            return _switchRequestRepository.CheckContains(m => m.Id == userid && m.Status == null);
        }


        public void DeleteSwitchRequest(string id)
        {
            _switchRequestRepository.DeleteMulti(m => m.SwitchRequestId == id);
        }
        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }   

    }
}
