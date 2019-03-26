using System;
using System.Linq;
using WebApp.Data.Infrastructure;
using WebApp.Data.Repositories;
using WebApp.Model.Models;

namespace WebApp.Service.Services
{
    public interface IIndentureService
    {
        IQueryable<Indenture> GetIndentures();
        IQueryable<Indenture> GetIndenturesOfStudentById(string userid);
        IQueryable<Indenture> GetIndenturesOfStudentByName(string username);

        Indenture GetIndentureById(string id); 
        Indenture AddIndenture(Indenture indenture);
        Indenture UpdateIndenture(Indenture indenture);

        bool CheckHasIndenturesValid(string userid);

        void DeleteIndenture(string id);
        void SaveChanges();
    }
    public class IndentureService : IIndentureService
    {
        private IUnitOfWork _unitOfWork;
        private IIndentureRepository _indentureRepository;

        public IndentureService(IUnitOfWork unitOfWork, IIndentureRepository indentureRepository)
        {
            this._unitOfWork = unitOfWork;
            this._indentureRepository = indentureRepository;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        public IQueryable<Indenture> GetIndentures()
        {
            return _indentureRepository.GetAll(new string[] { "AppUser", "Room" }).OrderByDescending(m => m.CreatedDate);
        }
        public IQueryable<Indenture> GetIndenturesOfStudentById(string userid)
        {
            return _indentureRepository.GetMulti(m => m.Id == userid, new string[] { "AppUser", "Room" }).OrderByDescending(m => m.CreatedDate);
        }
        public IQueryable<Indenture> GetIndenturesOfStudentByName(string username)
        {
            return _indentureRepository.GetMulti(m => m.AppUser.UserName == username, new string[] { "AppUser", "Room" }).OrderByDescending(m => m.CreatedDate);
        }


        public Indenture GetIndentureById(string id)
        {
            return _indentureRepository.GetSingleByCondition(m => m.IndentureId == id, new string[] { "AppUser", "Room" });
        }
        public Indenture AddIndenture(Indenture indenture)
        {
            return _indentureRepository.Add(indenture);
        }
        public Indenture UpdateIndenture(Indenture indenture)
        {
            return _indentureRepository.Update(indenture);
        }


        public bool CheckHasIndenturesValid(string userid)
        {
            var dateNow = DateTime.Now;
            return _indentureRepository.CheckContains(m => m.Id == userid && !m.IsCanceled && m.DateTo >= dateNow);
        }


        public void DeleteIndenture(string id)
        {
            _indentureRepository.DeleteMulti(m => m.IndentureId == id);
        }
        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }   

    }
}
