using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using Microsoft.EntityFrameworkCore;

namespace DelightBistroMvc.Data.Repositories
{
    public class ChatMessageRepository : BaseRepository<ChatMessageData>, IChatMessageRepository
    {
        public ChatMessageRepository(WebContext context) : base(context)
        {
        }

        public async Task<List<ChatMessageData>> GetRecentAsync(
            int count,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .OrderByDescending(m => m.CreatedAtUtc)
                .Take(count)
                .OrderBy(m => m.CreatedAtUtc)
                .ToListAsync(cancellationToken);
        }
    }
}
