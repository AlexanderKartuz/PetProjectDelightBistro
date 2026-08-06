using DelightBistroMinimalApi.Constans;
using DelightBistroMinimalApi.DbStuff;
using DelightBistroMinimalApi.Services.Cache.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace DelightBistroMinimalApi.Services.Database
{
    public class DrinksRedisCacheService : IDrinksCacheService
    {
        private DrinkRepository _teaRepository;
        private IDistributedCache _cache;

        public DrinksRedisCacheService(DrinkRepository teaRepository, IDistributedCache cache)
        {
            _cache = cache;
            _teaRepository = teaRepository;
        }

        public async Task<List<Tea>> GetDrinksAsync()
        {
            var key = CacheKeys.DRINKS;
            var teasCached = await _cache.GetStringAsync(key);

            if (teasCached != null)
            {
                var result = JsonSerializer.Deserialize<List<Tea>>(teasCached);
                if (result != null)
                {
                    return result;
                }

                _cache.Remove(key);
            }

            var teasDb = await _teaRepository.GetDrinksAsync();

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(2),
            };

            var teasJson = JsonSerializer.Serialize(teasDb);

            await _cache.SetStringAsync(key, teasJson, options);

            return teasDb;
        }

        public async Task<Tea?> GetDrinkAsync(int id)
        {
            var key = $"{CacheKeys.DRINK}:{id}";

            var teaCached = await _cache.GetStringAsync(key);

            if (teaCached != null)
            {
                var result = JsonSerializer.Deserialize<Tea>(teaCached);

                if (result != null)
                {
                    return result;
                }

                await _cache.RemoveAsync(key);
            }

            var teaDb = await _teaRepository.GetDrinkAsync(id);
            if (teaDb != null)
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(3),
                };

                var teaJson = JsonSerializer.Serialize(teaDb);
                await _cache.SetStringAsync(key, teaJson, options);
            }

            return teaDb;
        }

        public async Task CreateDrinkAsync(Tea tea)
        {
            await _teaRepository.CreateDrinkAsync(tea);
            await _cache.RemoveAsync(CacheKeys.DRINKS);
        }

        public async Task<Tea?> ChangeDrinkAsync(int id, Tea tea)
        {
            var changedTea = await _teaRepository.ChangeDrinkAsync(id, tea);
            if (changedTea != null)
            {
                await _cache.RemoveAsync(CacheKeys.DRINKS);
                await _cache.RemoveAsync($"{CacheKeys.DRINK}:{id}");
            }
            return changedTea;
        }

        public async Task<bool> DeleteDrinkAsync(int id)
        {
            var canDelete = await _teaRepository.DeleteDrinkAsync(id);
            if (!canDelete)
            {
                return false;
            }
            await _cache.RemoveAsync(CacheKeys.DRINKS);
            await _cache.RemoveAsync($"{CacheKeys.DRINK}:{id}");
            return true;
        }

    }
}