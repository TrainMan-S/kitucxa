using System.Linq;
using WebApp.Data.Infrastructure;
using WebApp.Data.Repositories;
using WebApp.Model.Models;

namespace WebApp.Service.Services
{
    public interface IFeedbackAnswerService
    {
        IQueryable<FeedbackAnswer> GetFeedbackAnswers();
        IQueryable<FeedbackAnswer> GetAnswersOfFeedback(string id);

        FeedbackAnswer GetFeedbackAnswerById(string id); 
        FeedbackAnswer AddFeedbackAnswer(FeedbackAnswer feedbackAnswer);
        FeedbackAnswer UpdateFeedbackAnswer(FeedbackAnswer feedbackAnswer);

        void DeleteFeedbackAnswer(string id);
        void SaveChanges();
    }
    public class FeedbackAnswerService : IFeedbackAnswerService
    {
        private IUnitOfWork _unitOfWork;
        private IFeedbackAnswerRepository _feedbackAnswerRepository;

        public FeedbackAnswerService(IUnitOfWork unitOfWork, IFeedbackAnswerRepository feedbackAnswerRepository)
        {
            this._unitOfWork = unitOfWork;
            this._feedbackAnswerRepository = feedbackAnswerRepository;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        public IQueryable<FeedbackAnswer> GetFeedbackAnswers()
        {
            return _feedbackAnswerRepository.GetAll().OrderByDescending(m => m.AnsweredDate);
        }
        public IQueryable<FeedbackAnswer> GetAnswersOfFeedback(string id)
        {
            return _feedbackAnswerRepository.GetMulti(m => m.FeedbackId == id).OrderByDescending(m => m.AnsweredDate);
        }

        public FeedbackAnswer GetFeedbackAnswerById(string id)
        {
            return _feedbackAnswerRepository.GetSingleById(id);
        }
        public FeedbackAnswer AddFeedbackAnswer(FeedbackAnswer feedbackAnswer)
        {
            return _feedbackAnswerRepository.Add(feedbackAnswer);
        }
        public FeedbackAnswer UpdateFeedbackAnswer(FeedbackAnswer feedbackAnswer)
        {
            return _feedbackAnswerRepository.Update(feedbackAnswer);
        }


        public void DeleteFeedbackAnswer(string id)
        {
            _feedbackAnswerRepository.DeleteMulti(m => m.FeedbackAnswerId == id);
        }
        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }   

    }
}
