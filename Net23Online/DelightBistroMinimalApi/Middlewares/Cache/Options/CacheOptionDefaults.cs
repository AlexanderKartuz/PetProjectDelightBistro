using DelightBistroMinimalApi.Middlewares.RateLimit.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.RateLimiting;

namespace DelightBistroMinimalApi.Middlewares.Cache.Options
{
    public class CacheOptionDefaults
    {
        private IDistributedCacheOptions _cacheOptions;
        public CacheOptionDefaults(IDistributedCacheOptions cacheOptions)
        {
            _cacheOptions = cacheOptions;
        }

        public DistributedCacheEntryOptions CreateDistributedCacheEntryOptions(IDistributedCacheOptions cacheOptions)
        {
            return new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(cacheOptions.AbsoluteMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(cacheOptions.SlidingMinutes),
            };

        }
    }
}
