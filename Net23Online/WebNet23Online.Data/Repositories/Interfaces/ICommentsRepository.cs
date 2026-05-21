using WebNet23Online.Data.Models;

namespace WebNet23Online.Data.Repositories.Interfaces
{
    public interface ICommentsRepository
    {
        List<CommentData> GetZooComments(int zooId);
    }
}
