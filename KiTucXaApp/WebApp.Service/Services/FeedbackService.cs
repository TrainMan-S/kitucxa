using System.Linq;
using WebApp.Data.Infrastructure;
using WebApp.Data.Repositories;
using WebApp.Model.Models;

namespace WebApp.Service.Services
{
    public interface IFeedbackService
    {
        IQueryable<Feedback> GetFeedbacks();
        IQueryable<Feedback> GetFeedbacksOfStudent(string username);

        Feedback GetFeedbackById(string id); 
        Feedback AddFeedback(Feedback feedback);
        Feedback UpdateFeedback(Feedback feedback);

        void DeleteFeedback(string id);
        void SaveChanges();
    }
    public class FeedbackService : IFeedbackService
    {
        private IUnitOfWork _unitOfWork;
        private IFeedbackRepository _feedbackRepository;
        private IFeedbackAnswerRepository _feedbackAnswerRepository;

        public FeedbackService(
            IUnitOfWork unitOfWork, 
            IFeedbackRepository feedbackRepository,
            IFeedbackAnswerRepository feedbackAnswerRepository)
        {
            this._unitOfWork = unitOfWork;
            this._feedbackRepository = feedbackRepository;
            this._feedbackAnswerRepository = feedbackAnswerRepository;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        public IQueryable<Feedback> GetFeedbacks()
        {
            return _feedbackRepository.GetAll().OrderByDescending(m => m.CreatedDate);
        }
        public IQueryable<Feedback> GetFeedbacksOfStudent(string userId)
        {
            return _feedbackRepository.GetMulti(m => m.StudentId == userId).OrderByDescending(m => m.CreatedDate);
        }


        public Feedback GetFeedbackById(string id)
        {
            return _feedbackRepository.GetSingleById(id);
        }
        public Feedback AddFeedback(Feedback feedback)
        {
            return _feedbackRepository.Add(feedback);
        }
        public Feedback UpdateFeedback(Feedback feedback)
        {
            return _feedbackRepository.Update(feedback);
        }


        public void DeleteFeedback(string id)
        {
            _feedbackAnswerRepository.DeleteMulti(m => m.FeedbackId == id);
            _feedbackRepository.DeleteMulti(m => m.FeedbackId == id);
        }
        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }   

    }
}
