using DelightBistroMinimalApi.DbStuff.Repositories;
using DelightBistroMinimalApi.DbStuff.Repositories.Interfaces;

namespace DelightBistroMinimalApi.Services.Background
{
    public class SerilogCleanupBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public SerilogCleanupBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                var loggerRepository = scope.ServiceProvider
                    .GetRequiredService<ISeriLogRepository>();

                await loggerRepository.CleanupAsync(stoppingToken);

                await Task.Delay(TimeSpan.FromDays(10), stoppingToken);
            }

        }
    }
}
