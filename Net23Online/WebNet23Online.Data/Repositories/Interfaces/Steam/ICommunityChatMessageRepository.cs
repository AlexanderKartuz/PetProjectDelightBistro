
using WebNet23Online.Data.Models.Steam;

namespace WebNet23Online.Data.Repositories.Interfaces.Steam
{
    public interface ICommunityChatMessageRepository : IBaseRepository<CommunityChatMessageData>
    {
        List<CommunityChatMessageData> GetAllMessagesWithUsers();
    }
}
