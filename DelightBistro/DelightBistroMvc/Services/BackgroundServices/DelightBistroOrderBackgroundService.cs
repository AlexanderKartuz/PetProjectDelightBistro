using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;

namespace DelightBistroMvc.Services.BackgroundServices
{
    public class DelightBistroOrderBackgroundService : BackgroundService
    {
        private const int DELAY_BETWEEN_ORDER_TIME_CHECK = 24 * 60 * 60; // one day
        private IServiceProvider _serviceProvider;

        public DelightBistroOrderBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {


            while (!stoppingToken.IsCancellationRequested)
            {
                using var di = _serviceProvider.CreateScope();
                var orderRepository = di.ServiceProvider.GetRequiredService<IOrderRepository>();

                orderRepository.DeleteExpiredOrderDatas();

                await Task.Delay(DELAY_BETWEEN_ORDER_TIME_CHECK * 1000, stoppingToken);
            }
        }
    }
}

