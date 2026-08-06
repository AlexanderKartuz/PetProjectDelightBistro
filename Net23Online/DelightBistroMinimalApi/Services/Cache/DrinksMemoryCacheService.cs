using DelightBistroMinimalApi.Constans;
using DelightBistroMinimalApi.DbStuff;
using DelightBistroMinimalApi.Services.Cache.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace DelightBistroMinimalApi.Services.Cache
{
    public class DrinksMemoryCacheService : IDrinksCacheService
    {
        private readonly IDrinkRepository _drinkRepository;
        private readonly IMemoryCache _cache;

        public DrinksMemoryCacheService(IDrinkRepository teaRepository, IMemoryCache cache)
        {
            _drinkRepository = teaRepository;
            _cache = cache;
        }

        public Task<Tea?> ChangeDrinkAsync(int id, Tea drink)
        {
            var changedDrink = _drinkRepository.ChangeDrinkAsync(id, drink);

            if (changedDrink != null)
            {
                _cache.Remove(CacheKeys.DRINKS);
                _cache.Remove($"CacheKeys.DRINK:{id}");
            }
            return changedDrink;
        }

        public async Task CreateDrinkAsync(Tea drink)
        {
            await _drinkRepository.CreateDrinkAsync(drink);
            _cache.Remove(CacheKeys.DRINKS);
        }

        public async Task<bool> DeleteDrinkAsync(int id)
        {
            if (!await _drinkRepository.DeleteDrinkAsync(id))
            {
                return false;
            }

            _cache.Remove(CacheKeys.DRINKS);
            _cache.Remove($"CacheKeys.DRINK:{id}");
            return true;
        }

        public async Task<Tea?> GetDrinkAsync(int id)
        {
            var cacheKey = $"{CacheKeys.DRINK}:{id}";

            if (!_cache.TryGetValue(cacheKey, out Tea? drink))
            {
                drink = await _drinkRepository.GetDrinkAsync(id);

                if (drink != null)
                {
                    _cache.Set(cacheKey, drink, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                        SlidingExpiration = TimeSpan.FromMinutes(2)
                    });
                }
            }
            return drink;
        }

        public async Task<List<Tea>> GetDrinksAsync()
        {
            var dinks = await _cache.GetOrCreateAsync(CacheKeys.DRINKS, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                entry.SlidingExpiration = TimeSpan.FromMinutes(2);

                var result = await _drinkRepository.GetDrinksAsync();

                return result ?? new List<Tea>();
            });
            return dinks;
        }
    }
}
