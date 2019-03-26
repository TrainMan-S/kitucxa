using WebApp.Data.Infrastructure;
using WebApp.Model.Models;

namespace WebApp.Data.Repositories
{
    public interface IRoomTypeRepository : IRepository<RoomType>
    {

    }

    public class RoomTypeRepository : RepositoryBase<RoomType>, IRoomTypeRepository
    {
        public RoomTypeRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }


    }
}
