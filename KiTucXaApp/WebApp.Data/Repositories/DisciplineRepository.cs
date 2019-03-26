using WebApp.Data.Infrastructure;
using WebApp.Model.Models;

namespace WebApp.Data.Repositories
{
    public interface IDisciplineRepository : IRepository<Discipline>
    {

    }

    public class DisciplineRepository : RepositoryBase<Discipline>, IDisciplineRepository
    {
        public DisciplineRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

    }
}
