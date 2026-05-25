using WebNet23Online.Data.Models;

namespace WebNet23Online.Data.Repositories.Interfaces
{
    public interface ICommentsRepository : IBaseRepository<CommentData>
    {
        List<CommentData> GetZooComments(int zooId);
    }
}
