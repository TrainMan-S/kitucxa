using System.Linq;
using WebApp.Data.Infrastructure;
using WebApp.Data.Repositories;
using WebApp.Model.Models;

namespace WebApp.Service.Services
{
    public interface IDisciplineService
    {
        IQueryable<Discipline> GetDisciplines();
        IQueryable<Discipline> GetDisciplinesOfStudentById(string userId);
        IQueryable<Discipline> GetDisciplinesOfStudentByName(string username);

        Discipline GetDisciplineById(string id); 
        Discipline AddDiscipline(Discipline discipline);
        Discipline UpdateDiscipline(Discipline discipline);

        void DeleteDiscipline(string id);
        void SaveChanges();
    }
    public class DisciplineService : IDisciplineService
    {
        private IUnitOfWork _unitOfWork;
        private IDisciplineRepository _disciplineRepository;

        public DisciplineService(IUnitOfWork unitOfWork, IDisciplineRepository disciplineRepository)
        {
            this._unitOfWork = unitOfWork;
            this._disciplineRepository = disciplineRepository;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        public IQueryable<Discipline> GetDisciplines()
        {
            return _disciplineRepository.GetAll(new string[] { "AppUser" }).OrderByDescending(m => m.CreatedDate);
        }
        public IQueryable<Discipline> GetDisciplinesOfStudentById(string userId)
        {
            return _disciplineRepository.GetMulti(m => m.Id == userId, new string[] { "AppUser" }).OrderByDescending(m => m.CreatedDate);
        }
        public IQueryable<Discipline> GetDisciplinesOfStudentByName(string username)
        {
            return _disciplineRepository.GetMulti(m => m.AppUser.UserName == username, new string[] { "AppUser" }).OrderByDescending(m => m.CreatedDate);
        }


        public Discipline GetDisciplineById(string id)
        {
            return _disciplineRepository.GetSingleByCondition(m => m.Id == id, new string[] { "AppUser" });
        }
        public Discipline AddDiscipline(Discipline discipline)
        {
            return _disciplineRepository.Add(discipline);
        }
        public Discipline UpdateDiscipline(Discipline discipline)
        {
            return _disciplineRepository.Update(discipline);
        }


        public void DeleteDiscipline(string id)
        {
            _disciplineRepository.DeleteMulti(m => m.DisciplineId == id);
        }
        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }   

    }
}
