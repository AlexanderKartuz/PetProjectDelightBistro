using WebNet23Online.Data.Repositories.Interfaces.Steam;

namespace WebNet23Online.Services.BackgroundServices.steam
{
    public class RatingAnalyticsBackgroundService : BackgroundService
    {
        public readonly TimeSpan DelayBetweenRatingRecalculation =  TimeSpan.FromMinutes(10);

        private readonly IServiceProvider _serviceProvider;

        public RatingAnalyticsBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                RecalculateGameRatings();
                await Task.Delay(DelayBetweenRatingRecalculation, stoppingToken);
            }
        }

        private void RecalculateGameRatings()
        {
            using var scope = _serviceProvider.CreateScope();
            var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
            var games = gameRepository.GetAllWithReviews();

            foreach (var game in games)
            {
                var reviews = game.GameReviews;

                game.ReviewsCount = reviews.Count;
                game.PositiveReviewsCount = reviews.Count(review => review.Rating >= 7);
                game.AverageRating = reviews.Any()
                    ? reviews.Average(review => review.Rating)
                    : null;
            }

            gameRepository.Update(games);
        }
    }
}
