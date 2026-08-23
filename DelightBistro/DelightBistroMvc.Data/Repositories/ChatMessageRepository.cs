using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;

namespace DelightBistroMvc.Data.Repositories
{
    public class ChatMessageRepository : BaseRepository<ChatMessageData>, IChatMessageRepository
    {
        public ChatMessageRepository(WebContext context) : base(context)
        {
        }

        public List<ChatMessageData> GetRecent(int count)
        {
            return _dbSet
                .OrderByDescending(m => m.CreatedAtUtc)
                .Take(count)
                .OrderBy(m => m.CreatedAtUtc)
                .ToList();
        }
    }
}
