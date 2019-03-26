using System;
using System.Linq;
using WebApp.Data.Infrastructure;
using WebApp.Data.Repositories;
using WebApp.Model.Models;

namespace WebApp.Service.Services
{
    public interface IRoomService
    {
        IQueryable<Room> GetRooms();
        IQueryable<Room> GetRoomsActive();

        Room GetRoomById(int id);
        Room GetRoomByCode(string roomCode);
        Room AddRoom(Room room);
        Room UpdateRoom(Room room);

        int CountCapacityNowOfRoom(int id);

        bool CheckRoomActiveById(int roomid);
        bool CheckRoomActiveByCode(string roomCode);
        bool CheckRoomExistById(int id);
        bool CheckRoomExistByCode(string roomCode);

        void DeleteRoom(int id);
        void SaveChanges();
    }
    public class RoomService : IRoomService
    {
        private IUnitOfWork _unitOfWork;
        private IRoomRepository _roomRepository;

        public RoomService(IUnitOfWork unitOfWork, IRoomRepository roomRepository)
        {
            this._unitOfWork = unitOfWork;
            this._roomRepository = roomRepository;
        }

        //**********************************************************************************
        //**********************************************************************************
        //**********************************************************************************

        public IQueryable<Room> GetRooms()
        {
            return _roomRepository.GetAll(new string[] { "RoomType" }).OrderBy(m => m.SortOrder);
        }
        public IQueryable<Room> GetRoomsActive()
        {
            return _roomRepository.GetMulti(m => m.IsActived, new string[] { "RoomType" }).OrderBy(m => m.SortOrder);
        }


        public Room GetRoomById(int id)
        {
            return _roomRepository.GetSingleByCondition(m => m.RoomId == id, new string[] { "RoomType" });
        }
        public Room GetRoomByCode(string roomCode)
        {
            return _roomRepository.GetSingleByCondition(m => m.Code == roomCode, new string[] { "RoomType" });
        }
        public Room AddRoom(Room room)
        {
            return _roomRepository.Add(room);
        }
        public Room UpdateRoom(Room room)
        {
            return _roomRepository.Update(room);
        }


        public int CountCapacityNowOfRoom(int id)
        {
            return _roomRepository.CountCapacityNowOfRoom(id);
        }


        public bool CheckRoomActiveById(int roomid)
        {
            return _roomRepository.CheckContains(m => m.RoomId == roomid && m.IsActived);
        }
        public bool CheckRoomActiveByCode(string roomCode)
        {
            return _roomRepository.CheckContains(m => m.Code == roomCode && m.IsActived);
        }
        public bool CheckRoomExistById(int id)
        {
            return _roomRepository.CheckContains(m => m.RoomId == id);
        }
        public bool CheckRoomExistByCode(string roomcode)
        {
            return _roomRepository.CheckContains(m => m.Code == roomcode);
        }


        public void DeleteRoom(int id)
        {
            _roomRepository.DeleteMulti(m => m.RoomId == id);
        }
        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }   

    }
}
