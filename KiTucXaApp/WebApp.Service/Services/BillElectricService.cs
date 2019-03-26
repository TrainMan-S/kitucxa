using System.Linq;
using WebApp.Data.Infrastructure;
using WebApp.Data.Repositories;
using WebApp.Model.Models;

namespace WebApp.Service.Services
{
    public interface IBillElectricService
    {
        IQueryable<BillElectric> GetBillElectrics();
        IQueryable<BillElectric> GetBillElectricsByRoomId(int roomid);
        IQueryable<BillElectric> GetBillElectricsByRoomCode(string roomcode);

        BillElectric GetBillElectricById(string id); 
        BillElectric AddBillElectric(BillElectric billElectric);
        BillElectric UpdateBillElectric(BillElectric billElectric);

        void DeleteBillElectric(string id);
        void SaveChanges();
    }
    public class BillElectricService : IBillElectricService
    {
        private IUnitOfWork _unitOfWork;
        private IBillElectricRepository _billElectricRepository;

        public BillElectricService(
            IUnitOfWork unitOfWork, 
            IBillElectricRepository billElectricRepository)
        {
            this._unitOfWork = unitOfWork;
            this._billElectricRepository = billElectricRepository;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        public IQueryable<BillElectric> GetBillElectrics()
        {
            return _billElectricRepository.GetAll(new string[] { "Room" }).OrderByDescending(m => m.CreatedDate);
        }
        public IQueryable<BillElectric> GetBillElectricsByRoomId(int roomid)
        {
            return _billElectricRepository.GetMulti(m => m.RoomId == roomid , new string[] { "Room" }).OrderByDescending(m => m.CreatedDate);
        }
        public IQueryable<BillElectric> GetBillElectricsByRoomCode(string roomcode)
        {
            return _billElectricRepository.GetMulti(m => m.Room.Code == roomcode, new string[] { "Room" }).OrderByDescending(m => m.CreatedDate);
        }


        public BillElectric GetBillElectricById(string id)
        {
            return _billElectricRepository.GetSingleByCondition(m => m.BillElectricId == id, new string[] { "Room" });
        }
        public BillElectric AddBillElectric(BillElectric billElectric)
        {
            return _billElectricRepository.Add(billElectric);
        }
        public BillElectric UpdateBillElectric(BillElectric billElectric)
        {
            return _billElectricRepository.Update(billElectric);
        }


        public void DeleteBillElectric(string id)
        {
            _billElectricRepository.DeleteMulti(m => m.BillElectricId == id);
        }
        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }   

    }
}
