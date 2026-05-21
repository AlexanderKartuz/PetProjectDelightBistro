using WebNet23Online.Models.Comments;

namespace WebNet23Online.Services.Interfaces
{
    public interface ICommentsService
    {
        AllCommentsViewModel GetZooComments(int zooId);

        bool AddZooComment(int zooId, string text);
    }
}
