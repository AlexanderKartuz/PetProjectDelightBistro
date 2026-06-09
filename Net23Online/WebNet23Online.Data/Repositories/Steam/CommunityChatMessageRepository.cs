
using Microsoft.EntityFrameworkCore;
using WebNet23Online.Data.Models.Steam;
using WebNet23Online.Data.Repositories.Interfaces.Steam;

namespace WebNet23Online.Data.Repositories.Steam
{
    public class CommunityChatMessageRepository : BaseRepository<CommunityChatMessageData>, ICommunityChatMessageRepository
    {
        public CommunityChatMessageRepository(WebContext context) : base(context)
        {
        }

        public List<CommunityChatMessageData> GetAllMessagesWithUsers()
        {
            return _dbSet
                .Include(x => x.CreatedByUser)
                .ToList();
        }
    }
}
