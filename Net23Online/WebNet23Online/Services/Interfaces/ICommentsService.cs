using WebNet23Online.Models.Comments;

namespace WebNet23Online.Services.Interfaces
{
    public interface ICommentsService
    {
        AllCommentsViewModel GetZooComments(int zooId);

        OneCommentViewModel AddZooComment(int zooId, string text);
    }
}
