using System.Linq;
using WebApp.Data.Infrastructure;
using WebApp.Data.Repositories;
using WebApp.Model.Models;

namespace WebApp.Service.Services
{
    public interface IRoomTypeService
    {
        IQueryable<RoomType> GetRoomTypes();
        IQueryable<RoomType> GetRoomTypesActive();

        RoomType GetRoomTypeById(int id); 
        RoomType AddRoomType(RoomType roomType);
        RoomType UpdateRoomType(RoomType roomType);

        bool CheckRoomTypeActive(int id);

        void DeleteRoomType(int id);
        void SaveChanges();
    }
    public class RoomTypeService : IRoomTypeService
    {
        private IUnitOfWork _unitOfWork;
        private IRoomTypeRepository _roomTypeRepository;

        public RoomTypeService(
            IUnitOfWork unitOfWork, 
            IRoomTypeRepository roomTypeRepository)
        {
            this._unitOfWork = unitOfWork;
            this._roomTypeRepository = roomTypeRepository;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        public IQueryable<RoomType> GetRoomTypes()
        {
            return _roomTypeRepository.GetAll().OrderBy(m => m.SortOrder);
        }
        public IQueryable<RoomType> GetRoomTypesActive()
        {
            return _roomTypeRepository.GetMulti(m => m.IsActived).OrderBy(m => m.SortOrder);
        }

        public RoomType GetRoomTypeById(int id)
        {
            return _roomTypeRepository.GetSingleById(id);
        }
        public RoomType AddRoomType(RoomType roomType)
        {
            return _roomTypeRepository.Add(roomType);
        }
        public RoomType UpdateRoomType(RoomType roomType)
        {
            return _roomTypeRepository.Update(roomType);
        }


        public bool CheckRoomTypeActive(int id)
        {
            return _roomTypeRepository.CheckContains(m => m.RoomTypeId == id && m.IsActived);
        }


        public void DeleteRoomType(int id)
        {
            _roomTypeRepository.DeleteMulti(m => m.RoomTypeId == id);
        }
        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }   

    }
}
