using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro
{
    public interface IChatMessageRepository : IBaseRepository<ChatMessageData>
    {
        /// <summary>
        /// Последние сообщения по дате
        /// </summary>
        /// <returns></returns>
        Task<List<ChatMessageData>> GetRecentAsync(int count, CancellationToken cancellationToken = default);
    }
}
