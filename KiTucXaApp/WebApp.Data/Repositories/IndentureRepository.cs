using System.Linq;
using WebApp.Data.Infrastructure;
using WebApp.Model.Models;

namespace WebApp.Data.Repositories
{
    public interface IIndentureRepository : IRepository<Indenture>
    {

    }

    public class IndentureRepository : RepositoryBase<Indenture>, IIndentureRepository
    {
        public IndentureRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }
}
