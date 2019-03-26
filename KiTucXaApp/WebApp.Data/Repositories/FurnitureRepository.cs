using WebApp.Data.Infrastructure;
using WebApp.Model.Models;

namespace WebApp.Data.Repositories
{
    public interface IFurnitureRepository : IRepository<Furniture>
    {

    }

    public class FurnitureRepository : RepositoryBase<Furniture>, IFurnitureRepository
    {
        public FurnitureRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

    }
}
