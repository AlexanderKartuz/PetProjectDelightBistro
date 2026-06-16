using DelightBistroMinimalApi.Constans;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace DelightBistroMinimalApi.DbStuff
{
    public class TeaService
    {
        private TeaRepository _teaRepository;
        private IDistributedCache _cache;

        public TeaService(TeaRepository teaRepository, IDistributedCache cache)
        {
            _cache = cache;
            _teaRepository = teaRepository;
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

            var teasDb = _teaRepository.GetTeas();

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

            var teaDb = _teaRepository.GetTea(id);
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

        public void CreateTea(Tea tea)
        {
            _teaRepository.CreateTea(tea);
            _cache.Remove(CacheKeys.TEAS);
        }

        public Tea? ChangeTea(int id, Tea tea)
        {
            var changedTea = _teaRepository.ChangeTea(id, tea);
            if (changedTea != null)
            {
                _cache.Remove(CacheKeys.TEAS);
                _cache.Remove($"{CacheKeys.TEA}:{id}");
            }
            return changedTea;
        }

        public bool DeleteTea(int id)
        {
            var canDelete=_teaRepository.DeleteTea(id);
            if (!canDelete)
            {
                return false;
            }
            _cache.Remove(CacheKeys.TEAS);
            _cache.Remove($"{CacheKeys.TEA}:{id}");
            return true;
        }
    }
}