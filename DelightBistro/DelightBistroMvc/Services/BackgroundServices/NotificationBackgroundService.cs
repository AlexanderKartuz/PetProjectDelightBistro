using Microsoft.AspNetCore.SignalR;
using DelightBistroMvc.Hubs.Interfaces;
using DelightBistroMvc.Hubs;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Data.Repositories.Interfaces;

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

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using var di = _serviceProvider.CreateScope();
                var serviceProvider = di.ServiceProvider;

                var notificationRepository = serviceProvider.GetRequiredService<INotificationRepository>();
                var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
                var hub = serviceProvider.GetRequiredService<IHubContext<NotificationHub, INotificationHub>>();

                var notificationsToShow = await notificationRepository
                    .GetReadyToPublishAsync(cancellationToken);

                if (notificationsToShow.Count > 0)
                {

                    foreach (var notification in notificationsToShow)
                    {
                        await hub.Clients.All.NewMessage(notification.Text);
                        notification.IsActive = false;
                    }

                    await notificationRepository.UpdateAsync(notificationsToShow, cancellationToken);
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                }

                await Task.Delay(DELAY_BETWEEN_NOTIFICATION_CHECK * 1000, cancellationToken);
            }
        }
    }
}
