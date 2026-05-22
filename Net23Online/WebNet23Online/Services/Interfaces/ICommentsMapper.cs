using WebNet23Online.Data.Models;
using WebNet23Online.Models.Comments;

namespace WebNet23Online.Services.Interfaces
{
    public interface ICommentsMapper
    {
        AllCommentsViewModel FromCommentsDataToCommnetsViewModel(List<CommentData> comments, int zooId);
    }
}
