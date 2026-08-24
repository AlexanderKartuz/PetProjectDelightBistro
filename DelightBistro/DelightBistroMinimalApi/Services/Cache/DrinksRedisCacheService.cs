using DelightBistroMinimalApi.Constans;
using DelightBistroMinimalApi.DbStuff.Models;
using DelightBistroMinimalApi.DbStuff.Repositories.Interfaces;
using DelightBistroMinimalApi.Services.Cache.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace DelightBistroMinimalApi.Services.Cache
{
    public class DrinksRedisCacheService : IDrinksCacheService
    {
        private IDrinkRepository _teaRepository;
        private IDistributedCache _cache;

        public DrinksRedisCacheService(IDrinkRepository drinkRepository, IDistributedCache cache)
        {
            _cache = cache;
            _teaRepository = drinkRepository;
        }

        public async Task<List<Drink>> GetDrinksAsync()
        {
            var key = CacheKeys.DRINKS;
            var teasCached = await _cache.GetStringAsync(key);

            if (teasCached != null)
            {
                var result = JsonSerializer.Deserialize<List<Drink>>(teasCached);
                if (result != null)
                {
                    return result;
                }

                await _cache.RemoveAsync(key);
            }

            var drinkDb = await _teaRepository.GetDrinksAsync();

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(2),
            };

            var drinkJson = JsonSerializer.Serialize(drinkDb);

            await _cache.SetStringAsync(key, drinkJson, options);

            return drinkDb;
        }

        public async Task<Drink?> GetDrinkAsync(int id)
        {
            var key = $"{CacheKeys.DRINK}:{id}";

            var drinkCached = await _cache.GetStringAsync(key);

            if (drinkCached != null)
            {
                var result = JsonSerializer.Deserialize<Drink>(drinkCached);

                if (result != null)
                {
                    return result;
                }

                await _cache.RemoveAsync(key);
            }

            var drinkDb = await _teaRepository.GetDrinkAsync(id);
            if (drinkDb != null)
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(3),
                };

                var drinkJson = JsonSerializer.Serialize(drinkDb);
                await _cache.SetStringAsync(key, drinkJson, options);
            }

            return drinkDb;
        }

        public async Task CreateDrinkAsync(Drink tea)
        {
            var task1 = _teaRepository.CreateDrinkAsync(tea);
            var task2 = _cache.RemoveAsync(CacheKeys.DRINKS);

            await Task.WhenAll(task1, task2);
        }

        public async Task<Drink?> ChangeDrinkAsync(int id, Drink tea)
        {
            var changedDrink = await _teaRepository.ChangeDrinkAsync(id, tea);
            if (changedDrink != null)
            {
                var task1 = _cache.RemoveAsync(CacheKeys.DRINKS);
                var task2 = _cache.RemoveAsync($"{CacheKeys.DRINK}:{id}");
                await Task.WhenAll(task1, task2);

            }
            return changedDrink;
        }

        public async Task<bool> DeleteDrinkAsync(int id)
        {
            var canDelete = await _teaRepository.DeleteDrinkAsync(id);
            if (!canDelete)
            {
                return false;
            }
            var task1 = _cache.RemoveAsync(CacheKeys.DRINKS);
            var task2 = _cache.RemoveAsync($"{CacheKeys.DRINK}:{id}");
            await Task.WhenAll(task1, task2);

            return true;
        }

    }
}