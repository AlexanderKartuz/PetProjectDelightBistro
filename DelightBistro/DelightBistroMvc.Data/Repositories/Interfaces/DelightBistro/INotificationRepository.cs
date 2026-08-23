using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;

public interface INotificationRepository : IBaseRepository<NotificationData>
{
    List<NotificationData> GetByLastNotifications();
}
