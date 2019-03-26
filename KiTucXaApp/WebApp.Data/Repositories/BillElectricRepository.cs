using WebApp.Data.Infrastructure;
using WebApp.Model.Models;

namespace WebApp.Data.Repositories
{
    public interface IBillElectricRepository : IRepository<BillElectric>
    {

    }

    public class BillElectricRepository : RepositoryBase<BillElectric>, IBillElectricRepository
    {
        public BillElectricRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

    }
}
