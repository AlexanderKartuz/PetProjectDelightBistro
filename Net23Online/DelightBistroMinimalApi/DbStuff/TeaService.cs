using DelightBistroMinimalApi.Constans;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace DelightBistroMinimalApi.DbStuff
{
    public class TeaService
    {
        private MiniDbContext _dbContext { get; set; }
        private IDistributedCache _cache { get; set; }
        public TeaService(MiniDbContext dbContext, IDistributedCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public List<Tea> GetTeas()
        {
            var key = CacheKeys.TEAS;

            var teasCached = _cache.GetString(key);

            if (teasCached != null)
            {
                var result = JsonSerializer.Deserialize<List<Tea>>(teasCached);
                return result;
            }

            var teasDb = _dbContext.Teas.ToList();

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(2),
                SlidingExpiration = TimeSpan.FromMinutes(1),
            };

            _cache.SetString(key, JsonSerializer.Serialize(teasDb), options);

            return teasDb;
        }

        public Tea? GetTea(int id)
        {
            var key = $"{CacheKeys.TEA}:{id}";

            var teaCached = _cache.GetString(key);

            if (teaCached != null)
            {
                var result = JsonSerializer.Deserialize<Tea>(teaCached);
                return result;
            }

            var teaDb = _dbContext.Teas.FirstOrDefault(t => t.Id == id);
            if (teaDb != null)
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(3),
                };

                _cache.SetString(key, JsonSerializer.Serialize(teaDb), options);
            }

            return teaDb;
        }

        public void RemoveTeaListCache()
        {
            _cache.Remove(CacheKeys.TEAS);
        }

        public void RemoveTeaCache(int id)
        {
            _cache.Remove($"{CacheKeys.TEA}:{id}");
        }
    }
}