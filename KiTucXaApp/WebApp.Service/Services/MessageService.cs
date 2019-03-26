using System.Linq;
using WebApp.Data.Infrastructure;
using WebApp.Data.Repositories;
using WebApp.Model.Models;

namespace WebApp.Service.Services
{
    public interface IMessageService
    {
        IQueryable<Message> GetMessages();

        Message GetMessageById(string id); 
        Message AddMessage(Message message);
        Message UpdateMessage(Message message);

        void DeleteMessage(string id);
        void SaveChanges();
    }
    public class MessageService : IMessageService
    {
        private IUnitOfWork _unitOfWork;
        private IMessageRepository _messageRepository;

        public MessageService(IUnitOfWork unitOfWork, IMessageRepository messageRepository)
        {
            this._unitOfWork = unitOfWork;
            this._messageRepository = messageRepository;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        public IQueryable<Message> GetMessages()
        {
            return _messageRepository.GetAll().OrderByDescending(m => m.CreatedDate);
        }


        public Message GetMessageById(string id)
        {
            return _messageRepository.GetSingleById(id);
        }
        public Message AddMessage(Message message)
        {
            return _messageRepository.Add(message);
        }
        public Message UpdateMessage(Message message)
        {
            return _messageRepository.Update(message);
        }


        public void DeleteMessage(string id)
        {
            _messageRepository.DeleteMulti(m => m.MessageId == id);
        }
        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }   

    }
}
