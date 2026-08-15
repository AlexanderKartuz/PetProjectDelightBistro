using Microsoft.EntityFrameworkCore;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces;

namespace DelightBistroMvc.Data.Repositories;

public class NotificationRepository : BaseRepository<NotificationData>,
    INotificationRepository
{
    public NotificationRepository(WebContext context) : base(context)
    {
    }

    public override List<NotificationData> GetAll()
    {
        return _dbSet.Include(x => x.Author).ToList();
    }

    public List<NotificationData> GetByLastNotifications()
    {
        return _dbSet
            .Where(x => x.TimeToPublish < DateTime.UtcNow
                && x.IsActive)
            .ToList();
    }
}
