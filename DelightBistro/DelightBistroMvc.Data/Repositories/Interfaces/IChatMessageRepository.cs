using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Data.Repositories.Interfaces
{
    public interface IChatMessageRepository : IBaseRepository<ChatMessageData>
    {
        /// <summary>
        /// Последние сообщения по дате
        /// </summary>
        /// <returns></returns>
        List<ChatMessageData> GetRecent(int count);
    }
}
