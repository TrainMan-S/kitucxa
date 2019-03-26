using WebApp.Data.Infrastructure;
using WebApp.Model.Models;

namespace WebApp.Data.Repositories
{
    public interface ISwitchRequestRepository : IRepository<SwitchRequest>
    {

    }

    public class SwitchRequestRepository : RepositoryBase<SwitchRequest>, ISwitchRequestRepository
    {
        public SwitchRequestRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

    }
}
