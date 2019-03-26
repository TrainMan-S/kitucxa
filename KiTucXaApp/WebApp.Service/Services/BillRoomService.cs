using System.Linq;
using WebApp.Data.Infrastructure;
using WebApp.Data.Repositories;
using WebApp.Model.Models;

namespace WebApp.Service.Services
{
    public interface IBillRoomService
    {
        IQueryable<BillRoom> GetBillRooms();

        BillRoom GetBillRoomById(string id); 
        BillRoom AddBillRoom(BillRoom billRoom);
        BillRoom UpdateBillRoom(BillRoom billRoom);

        void DeleteBillRoom(string id);
        void SaveChanges();
    }
    public class BillRoomService : IBillRoomService
    {
        private IUnitOfWork _unitOfWork;
        private IBillRoomRepository _billRoomRepository;

        public BillRoomService(IUnitOfWork unitOfWork, IBillRoomRepository billRoomRepository)
        {
            this._unitOfWork = unitOfWork;
            this._billRoomRepository = billRoomRepository;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        public IQueryable<BillRoom> GetBillRooms()
        {
            return _billRoomRepository.GetAll().OrderByDescending(m => m.CreatedDate);
        }


        public BillRoom GetBillRoomById(string id)
        {
            return _billRoomRepository.GetSingleById(id);
        }
        public BillRoom AddBillRoom(BillRoom billRoom)
        {
            return _billRoomRepository.Add(billRoom);
        }
        public BillRoom UpdateBillRoom(BillRoom billRoom)
        {
            return _billRoomRepository.Update(billRoom);
        }


        public void DeleteBillRoom(string id)
        {
            _billRoomRepository.DeleteMulti(m => m.BillRoomId == id);
        }
        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }   

    }
}
