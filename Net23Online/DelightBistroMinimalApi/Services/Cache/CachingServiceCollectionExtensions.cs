using DelightBistroMinimalApi.Services.Cache.Interfaces;
using DelightBistroMinimalApi.Services.Cache.Options;
using DelightBistroMinimalApi.Services.Database;

namespace DelightBistroMinimalApi.Services.Cache
{
    /// <summary>
    /// Регистрация Memory/Redis кэша по секции Caching.
    /// </summary>
    public static class CachingServiceCollectionExtensions
    {
        public static CachingOptions AddDelightBistroCaching(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = new CachingOptions();
            configuration.GetSection(CachingOptions.SectionName).Bind(options);
            services.AddSingleton(options);

            services.AddMemoryCache();

            if (!options.UseRedis)
            {
                services.AddScoped<IDrinksCacheService, DrinksRedisCacheService>();
            }
            else
            {
                services.AddScoped<IDrinksCacheService, DrinksMemoryCacheService>();
            }

            var redisConnection = configuration.GetConnectionString("Redis");
            if (string.IsNullOrWhiteSpace(redisConnection))
            {
                throw new InvalidOperationException(
                    "ConnectionStrings:Redis обязателен, когда Caching:Provider = Redis.");
            }

            services.AddStackExchangeRedisCache(cacheOptions =>
            {
                cacheOptions.Configuration = redisConnection;
                cacheOptions.InstanceName = options.InstanceName;
            });

            services.AddScoped<DrinksRedisCacheService>();

            return options;
        }
    }
}
