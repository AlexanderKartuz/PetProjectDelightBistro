using Microsoft.AspNetCore.SignalR;
using DelightBistroMvc.Hubs.Interfaces;
using DelightBistroMvc.Hubs;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;

namespace DelightBistroMvc.Services.BackgroundServices
{
    public class NotificationBackgroundService : BackgroundService
    {
        public const int DELAY_BETWEEN_NOTIFICATION_CHECK = 30;
        private IServiceProvider _serviceProvider;

        public NotificationBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var di = _serviceProvider.CreateScope();
                var notificationRepository = di.ServiceProvider.GetRequiredService<INotificationRepository>();
                var hub = di.ServiceProvider.GetRequiredService<IHubContext<NotificationHub, INotificationHub>>();

                // var span = new TimeSpan(DELAY_BETWEEN_NOTIFICATION_CHECK * 10 * 1000 * 1000);
                var notificationsToShow = notificationRepository.GetByLastNotifications();

                if (notificationsToShow.Count > 0)
                {

                    foreach (var notification in notificationsToShow)
                    {
                        await hub.Clients.All.NewMessage(notification.Text);
                        notification.IsActive = false;
                    }

                    notificationRepository.Update(notificationsToShow);
                }

                await Task.Delay(DELAY_BETWEEN_NOTIFICATION_CHECK * 1000, stoppingToken);
            }
        }
    }
}
