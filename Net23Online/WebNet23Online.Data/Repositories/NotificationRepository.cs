using Microsoft.EntityFrameworkCore;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Data.Repositories;

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
            .Where(x => x.TimeToPublish < DateTime.Now
                && x.IsActive)
            .ToList();
    }
}
