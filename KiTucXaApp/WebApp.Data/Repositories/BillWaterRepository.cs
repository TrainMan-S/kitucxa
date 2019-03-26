using WebApp.Data.Infrastructure;
using WebApp.Model.Models;

namespace WebApp.Data.Repositories
{
    public interface IBillWaterRepository : IRepository<BillWater>
    {

    }

    public class BillWaterRepository : RepositoryBase<BillWater>, IBillWaterRepository
    {
        public BillWaterRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

    }
}
