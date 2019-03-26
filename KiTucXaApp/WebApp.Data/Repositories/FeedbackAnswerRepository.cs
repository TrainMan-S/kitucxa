using WebApp.Data.Infrastructure;
using WebApp.Model.Models;

namespace WebApp.Data.Repositories
{
    public interface IFeedbackAnswerRepository : IRepository<FeedbackAnswer>
    {

    }

    public class FeedbackAnswerRepository : RepositoryBase<FeedbackAnswer>, IFeedbackAnswerRepository
    {
        public FeedbackAnswerRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

    }
}
