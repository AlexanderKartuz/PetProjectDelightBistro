using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;

public interface INotificationRepository : IBaseRepository<NotificationData>
{
    Task<List<NotificationData>> GetReadyToPublishAsync(CancellationToken cancellationToken = default);
}
