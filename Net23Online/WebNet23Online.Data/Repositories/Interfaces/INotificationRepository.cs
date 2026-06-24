using WebNet23Online.Data.Models;

namespace WebNet23Online.Data.Repositories.Interfaces;

public interface INotificationRepository : IBaseRepository<NotificationData>
{
    List<NotificationData> GetByLastNotifications();
}
