using System.Linq;
using WebApp.Data.Infrastructure;
using WebApp.Data.Repositories;
using WebApp.Model.Models;

namespace WebApp.Service.Services
{
    public interface IBillWaterService
    {
        IQueryable<BillWater> GetBillWaters();
        IQueryable<BillWater> GetBillWatersByRoomId(int roomid);
        IQueryable<BillWater> GetBillWatersByRoomCode(string roomcode);

        BillWater GetBillWaterById(string id); 
        BillWater AddBillWater(BillWater billWater);
        BillWater UpdateBillWater(BillWater billWater);

        void DeleteBillWater(string id);
        void SaveChanges();
    }
    public class BillWaterService : IBillWaterService
    {
        private IUnitOfWork _unitOfWork;
        private IBillWaterRepository _billWaterRepository;

        public BillWaterService(IUnitOfWork unitOfWork, IBillWaterRepository billWaterRepository)
        {
            this._unitOfWork = unitOfWork;
            this._billWaterRepository = billWaterRepository;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        public IQueryable<BillWater> GetBillWaters()
        {
            return _billWaterRepository.GetAll(new string[] { "Room" }).OrderByDescending(m => m.CreatedDate);
        }
        public IQueryable<BillWater> GetBillWatersByRoomId(int roomid)
        {
            return _billWaterRepository.GetMulti(m => m.RoomId == roomid, new string[] { "Room" }).OrderByDescending(m => m.CreatedDate);
        }
        public IQueryable<BillWater> GetBillWatersByRoomCode(string roomcode)
        {
            return _billWaterRepository.GetMulti(m => m.Room.Code == roomcode, new string[] { "Room" }).OrderByDescending(m => m.CreatedDate);
        }


        public BillWater GetBillWaterById(string id)
        {
            return _billWaterRepository.GetSingleByCondition(m => m.BillWaterId == id, new string[] { "Room" });
        }
        public BillWater AddBillWater(BillWater billWater)
        {
            return _billWaterRepository.Add(billWater);
        }
        public BillWater UpdateBillWater(BillWater billWater)
        {
            return _billWaterRepository.Update(billWater);
        }


        public void DeleteBillWater(string id)
        {
            _billWaterRepository.DeleteMulti(m => m.BillWaterId == id);
        }
        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }   

    }
}
