using WebApp.Data.Infrastructure;
using WebApp.Model.Models;

namespace WebApp.Data.Repositories
{
    public interface IBillRoomRepository : IRepository<BillRoom>
    {

    }

    public class BillRoomRepository : RepositoryBase<BillRoom>, IBillRoomRepository
    {
        public BillRoomRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

    }
}
