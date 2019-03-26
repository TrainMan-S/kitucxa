using System;
using System.Linq;
using WebApp.Data.Infrastructure;
using WebApp.Model.Models;

namespace WebApp.Data.Repositories
{
    public interface IRoomRepository : IRepository<Room>
    {
        //// Đếm số người trong phòng hiện tại
        //int CountCapacityNowOfRoom(int id);

        //// Đếm số người trong phòng tại 1 thời điểm
        //int CountCapacityFutureOfRoom(int id, DateTime dateFrom);

        //// Đếm số người lơn nhất trong phòng trong 1 khoảng thời gian
        //int CountCapacityOnTimeOfRoom(int id, DateTime dateFrom, DateTime dateTo);

        // Đếm số người trong phòng hiện tại
        int CountCapacityNowOfRoom(int id);
    }

    public class RoomRepository : RepositoryBase<Room>, IRoomRepository
    {
        public RoomRepository(IDbFactory dbFactory) : base(dbFactory) { }

        //public int CountCapacityNowOfRoom(int id)
        //{
        //    var dateNow = DateTime.Now.Date;
        //    return DbContext.Indentures.Count(m => m.RoomId == id && m.Status <= 1 && m.DateFrom <= dateNow && m.DateTo >= dateNow);
        //}

        //public int CountCapacityFutureOfRoom(int id, DateTime dateFrom)
        //{
        //    return DbContext.Indentures
        //        .Where(m => m.RoomId == id && m.Status <= 1 && m.DateFrom <= dateFrom && m.DateTo > dateFrom)
        //        .Select(m => m.Id).Distinct().Count();
        //}

        //public int CountCapacityOnTimeOfRoom(int id, DateTime dateFrom, DateTime dateTo)
        //{
        //    return DbContext.Indentures
        //        .Where(m => m.RoomId == id && m.Status <= 1 && (( m.DateTo >= dateFrom && m.DateTo <= dateTo ) || ( m.DateFrom >= dateFrom && m.DateFrom <= dateTo ) || ( m.DateFrom <= dateFrom && m.DateTo >= dateTo)))
        //        .Select(m => m.Id).Distinct().Count();
        //}

        public int CountCapacityNowOfRoom(int id)
        {
            return DbContext.Users.Count(m => m.RoomId == id && m.GroupId == 5 && m.IsActived);
        }
    }
}
