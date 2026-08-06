using DelightBistro.Sevices.Logging;
using DelightBistroMinimalApi.Constans;
using DelightBistroMinimalApi.DbStuff;
using DelightBistroMinimalApi.Middlewares;
using DelightBistroMinimalApi.Middlewares.RateLimit;
using DelightBistroMinimalApi.Services.Cache;
using DelightBistroMinimalApi.Services.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

//service and appsetings
var connectionString = builder.Configuration.GetConnectionString("Drinks");
builder.Services.AddDbContext<MiniDbContext>(op => op.UseSqlServer(connectionString));
builder.Services.AddScoped<DrinkRepository>();

var cachingOptions = builder.Services.AddDelightBistroCaching(builder.Configuration);

builder.Services.AddScoped(typeof(IAppLogging<>), typeof(AppLogging<>));
builder.ConfigureSeriLog();

builder.AddCustomRateLimiter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(60);
    options.MaximumBodySize = 64 * 1024;
    options.SizeLimit = 100 * 1024 * 1024;
});

builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(p =>
    {
        p.AllowAnyHeader();
        p.AllowAnyMethod();
        p.SetIsOriginAllowed(x => true);
        p.AllowCredentials();
    });
});

var app = builder.Build();

app.UseCustomExeptionHandling();

app.UseCors();
app.UseRateLimiter();

app.UseResponseHeader();
app.UseOutputCache();
app.UseCustomRequestLogging();

app.UseSwagger();
app.UseSwaggerUI();

//service

app.MapGet("/", () => "Hello World!");

app.MapGet("GetTeas", (DrinkRepository teaRepository, IMemoryCache memoryCache) =>
{
    var teas = memoryCache.GetOrCreate(CacheKeys.DRINKS, async entry =>
    {
        entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(5);
        entry.SlidingExpiration = TimeSpan.FromMinutes(2);

        return await teaRepository.GetDrinksAsync();
    });

    return Results.Ok(teas);
}).CacheOutput(o => o.Tag(CacheTags.DRINKS));

app.MapGet("GetTea/{id}",
    async (DrinkRepository teaRepository,
    int id,
    IMemoryCache memoryCache) =>
{
    var cacheKey = $"{CacheKeys.DRINK}:{id}";

    //var tea = memoryCache.GetOrCreate(cacheKey, entry =>
    //{
    //    entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(10);
    //    entry.SlidingExpiration = TimeSpan.FromMinutes(2);

    //    return teaRepository.GetTea(id);
    //});

    if (!memoryCache.TryGetValue(cacheKey, out Tea? tea))
    {
        tea = await teaRepository.GetDrinkAsync(id);

        if (tea != null)
        {
            memoryCache.Set(cacheKey, tea, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            });
        }
    }

    if (tea == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(tea);
}).CacheOutput(o => o
.Tag(CacheTags.DRINK)
.SetVaryByRouteValue("id")
.Expire(TimeSpan.FromMinutes(1)));

app.MapPost("CreateTea",
    async (DrinkRepository teaRepository,
    [FromBody] Tea tea,
    IOutputCacheStore outputCache,
   IMemoryCache memoryCache) =>
{
    await teaRepository.CreateDrinkAsync(tea);

    memoryCache.Remove(CacheKeys.DRINKS);

    await outputCache.EvictByTagAsync(CacheTags.DRINKS, default);

    return Results.Ok(tea);
});

app.MapPut("ChangeDrink/{id}",
   async (DrinkRepository teaRepository,
   int id, [FromBody] Tea tea,
   IOutputCacheStore outputCache,
   IMemoryCache memoryCache) =>
{
    var changedTea = await teaRepository.ChangeDrinkAsync(id, tea);

    if (changedTea == null)
    {
        return Results.NotFound();
    }

    memoryCache.Remove(CacheKeys.DRINKS);
    memoryCache.Remove($"{CacheKeys.DRINK}:{id}");

    // from SetVaryByRouteValue
    var cacheKey = $"GET:/GetTea/{id}:routeId={id}";
    await outputCache.EvictByTagAsync(cacheKey, default);

    return Results.Ok(changedTea);
});

app.MapDelete("DeleteDrink",
    async (DrinkRepository teaRepository,
    [FromBody] int id,
    IOutputCacheStore outputCache,
    IMemoryCache memoryCache) =>
{
    var canDelete = await teaRepository.DeleteDrinkAsync(id);
    if (!canDelete)
    {
        return false;
    }

    memoryCache.Remove(CacheKeys.DRINKS);
    memoryCache.Remove($"{CacheKeys.DRINK}:{id}");

    var cacheKey = $"GET:/GetTea/{id}:routeId={id}";
    await outputCache.EvictByTagAsync(cacheKey, default);

    return true;
});

app.MapGet("Exception", () => { throw new Exception(); });

// Redis-эндпоинты только при Caching:Provider = Redis
if (cachingOptions.UseRedis)
{
    app.MapGet("redis-test", async (IDistributedCache cache) =>
    {
        var value = await cache.GetStringAsync("test");

        if (value == null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(1),
            };

            var initValue = "Redis is working!";
            await cache.SetStringAsync("test", initValue, options);

            return Results.Ok(initValue);
        }

        return Results.Ok(value);
    });

    app.MapGet("GetTeasRedis", async (DrinksRedisCacheService teaService) =>
    {
        var teas = await teaService.GetDrinksAsync();

        return Results.Ok(teas);
    }).CacheOutput(o => o.Tag(CacheTags.DRINKS));

    app.MapGet("GetTeaRedis/{id}",
        async (DrinksRedisCacheService teaService, int id) =>
    {
        var tea = await teaService.GetDrinkAsync(id);

        if (tea == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(tea);
    }).CacheOutput(o =>
    o.Tag(CacheTags.DRINK)
    .SetVaryByRouteValue("id")
    .Expire(TimeSpan.FromMinutes(2)));

    app.MapPost("CreateTeaRedis",
        async (DrinksRedisCacheService teaService,
        [FromBody] Tea tea,
        IOutputCacheStore outputCache) =>
    {
        teaService.CreateDrinkAsync(tea);

        await outputCache.EvictByTagAsync(CacheTags.DRINKS, default);

        return Results.Ok(tea);
    });

    app.MapPut("ChangeDrinkRedis/{id}",
       async (DrinksRedisCacheService teaService,
       int id, [FromBody] Tea tea,
       IOutputCacheStore outputCache) =>
       {
           var changedTea = teaService.ChangeTeaAsync(id, tea);

           if (changedTea == null)
           {
               return Results.NotFound();
           }

           await outputCache.EvictByTagAsync(CacheTags.DRINKS, default);

           return Results.Ok(changedTea);
       });

    app.MapDelete("DeleteDrinkRedis",
        async (DrinksRedisCacheService teaService,
        [FromBody] int id,
        IOutputCacheStore outputCache) =>
        {
            var canDelete = await teaService.DeleteDrinkAsync(id);
            if (!canDelete)
            {
                return false;
            }

            await outputCache.EvictByTagAsync(CacheTags.DRINKS, default);

            return true;
        });
}

app.Run();
