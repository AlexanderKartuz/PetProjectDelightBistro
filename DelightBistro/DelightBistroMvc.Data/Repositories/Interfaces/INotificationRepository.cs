using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Data.Repositories.Interfaces;

public interface INotificationRepository : IBaseRepository<NotificationData>
{
    List<NotificationData> GetByLastNotifications();
}
