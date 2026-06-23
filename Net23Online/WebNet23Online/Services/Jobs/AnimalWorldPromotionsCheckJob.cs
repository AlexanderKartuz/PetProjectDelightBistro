using Microsoft.AspNetCore.SignalR;
using Quartz;
using WebNet23Online.Data.Repositories.Interfaces.AnimalWorld;
using WebNet23Online.Hubs;

namespace WebNet23Online.Services.Jobs
{
    public class AnimalWorldPromotionsCheckJob : IJob
    {
        private IServiceScopeFactory _scopeFactory;

        public AnimalWorldPromotionsCheckJob(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var promotionsRepository = scope.ServiceProvider.GetRequiredService<IPromotionRepository>();
                var zooRepository = scope.ServiceProvider.GetRequiredService<IZooRepository>();
                var hub = scope.ServiceProvider.GetRequiredService<IHubContext<AnimalWorldNotificationsHub, IAnimalWorldNotificationsHub>>();
                var promotionsForDelete = new List<int>();
                var promotions = promotionsRepository.GetAll();
                foreach (var promotion in promotions)
                {
                    if (promotion.EndDate < DateTime.Now)
                    {
                        promotionsForDelete.Add(promotion.Id);
                    }
                    else
                    {
                        var zoo = zooRepository.Get(promotion.ZooId);
                        var message = $"В зоопарке {zoo.ZooName} проходит акция \"{promotion.PromotionName}\".\n\n{promotion.Description}\n\nАкция заканчивается {promotion.EndDate:yyyy-MM-dd}.";
                        await hub.Clients.All.ZoosPromotions(message);
                        Console.WriteLine($"[AnimalWorldPromotionsBackgroundService] Отправлено: {message}");
                    }
                }

                if (promotionsForDelete.Any())
                {
                    promotionsRepository.Delete(promotionsForDelete);
                }
            }
        }
    }
}
