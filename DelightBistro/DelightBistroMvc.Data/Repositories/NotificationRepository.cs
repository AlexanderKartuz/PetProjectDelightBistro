using Microsoft.EntityFrameworkCore;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;

namespace DelightBistroMvc.Data.Repositories;

public class NotificationRepository : BaseRepository<NotificationData>,
    INotificationRepository
{
    public NotificationRepository(WebContext context) : base(context)
    {
    }

    public override Task<List<NotificationData>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _dbSet.Include(x => x.Author).ToListAsync(cancellationToken);
    }

    public List<NotificationData> GetByLastNotifications()
    {
        return _dbSet
            .Where(x => x.TimeToPublish < DateTime.UtcNow
                && x.IsActive)
            .ToList();
    }
}
