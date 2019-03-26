using System.Linq;
using WebApp.Data.Infrastructure;
using WebApp.Data.Repositories;
using WebApp.Model.Models;

namespace WebApp.Service.Services
{
    public interface IFurnitureService
    {
        IQueryable<Furniture> GetFurnitures();
        IQueryable<Furniture> GetFurnituresByRoomId(int roomid);
        IQueryable<Furniture> GetFurnituresByRoomCode(string roomcode);

        Furniture GetFurnitureById(string id); 
        Furniture AddFurniture(Furniture furniture);
        Furniture UpdateFurniture(Furniture furniture);

        void DeleteFurniture(string id);
        void SaveChanges();
    }
    public class FurnitureService : IFurnitureService
    {
        private IUnitOfWork _unitOfWork;
        private IFurnitureRepository _furnitureRepository;

        public FurnitureService(IUnitOfWork unitOfWork, IFurnitureRepository furnitureRepository)
        {
            this._unitOfWork = unitOfWork;
            this._furnitureRepository = furnitureRepository;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        public IQueryable<Furniture> GetFurnitures()
        {
            return _furnitureRepository.GetAll(new string[] { "Room" }).OrderByDescending(m => m.CreatedDate);
        }
        public IQueryable<Furniture> GetFurnituresByRoomId(int roomid)
        {
            return _furnitureRepository.GetMulti(m => m.RoomId == roomid, new string[] { "Room" }).OrderByDescending(m => m.CreatedDate);
        }
        public IQueryable<Furniture> GetFurnituresByRoomCode(string roomcode)
        {
            return _furnitureRepository.GetMulti(m => m.Room.Code == roomcode, new string[] { "Room" }).OrderByDescending(m => m.CreatedDate);
        }


        public Furniture GetFurnitureById(string id)
        {
            return _furnitureRepository.GetSingleByCondition(m => m.FurnitureId == id, new string[] { "Room" });
        }
        public Furniture AddFurniture(Furniture furniture)
        {
            return _furnitureRepository.Add(furniture);
        }
        public Furniture UpdateFurniture(Furniture furniture)
        {
            return _furnitureRepository.Update(furniture);
        }


        public void DeleteFurniture(string id)
        {
            _furnitureRepository.DeleteMulti(m => m.FurnitureId == id);
        }
        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }   

    }
}
