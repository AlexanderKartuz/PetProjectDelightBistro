using Microsoft.AspNetCore.SignalR;
using DelightBistroMvc.Data.Repositories.Interfaces;
using DelightBistroMvc.Hubs.Interfaces;
using DelightBistroMvc.Hubs;

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
            using var di = _serviceProvider.CreateScope();
            var notificationRepository = di.ServiceProvider.GetService<INotificationRepository>();
            var hub = di.ServiceProvider.GetService<IHubContext<NotificationHub, INotificationHub>>();
            
            
            while (true)
            {
                // var span = new TimeSpan(DELAY_BETWEEN_NOTIFICATION_CHECK * 10 * 1000 * 1000);
                var notificationsToShow = notificationRepository.GetByLastNotifications();

                foreach (var notification in notificationsToShow)
                {
                    hub.Clients.All.NewMessage(notification.Text);
                    notification.IsActive = false;
                }

                notificationRepository.Update(notificationsToShow);

                await Task.Delay(DELAY_BETWEEN_NOTIFICATION_CHECK * 1000);
            }
        }
    }
}
