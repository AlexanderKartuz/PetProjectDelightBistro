using DelightBistroMinimalApi.Constans;
using DelightBistroMinimalApi.DbStuff;
using DelightBistroMinimalApi.Middlewares;
using DelightBistroMinimalApi.Middlewares.RateLimit;
using DelightBistroMinimalApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WebNet23Tea;Integrated Security=True;Connect Timeout=30;";
builder.Services.AddDbContext<MiniDbContext>(op => op.UseSqlServer(connectionString));
builder.Services.AddScoped<TeaCacheService>();
builder.Services.AddScoped<TeaRepository>();


builder.Services.AddCustomRateLimiter(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Кеш HTTP ответов  
builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(60);
    options.MaximumBodySize = 64 * 1024;
    options.SizeLimit = 100 * 1024 * 1024;
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "DelightBistro_";
});

builder.Services.AddMemoryCache();

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


// UseCors must be called before UseResponseCaching
app.UseCors();
app.UseRateLimiter();

//Cache
app.UseResponseHeader();
app.UseOutputCache();
app.UseCustomRequestLogging();


app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello World!");

app.MapGet("GetTeas", (TeaRepository teaRepository, IMemoryCache memoryCache) =>
{
    var teas = memoryCache.GetOrCreate(CacheKeys.TEAS, entry =>
    {
        entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(5);
        entry.SlidingExpiration = TimeSpan.FromMinutes(2);

        return teaRepository.GetTeas();
    });

    return Results.Ok(teas);
}).CacheOutput(o => o.Tag(CacheTags.TEAS));


app.MapGet("GetTea/{id}", (TeaRepository teaRepository, int id, IMemoryCache memoryCache) =>
{
    var cacheKey = $"{CacheKeys.TEA}:{id}";

    var tea = memoryCache.GetOrCreate(cacheKey, entry =>
    {
        entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(10);
        entry.SlidingExpiration = TimeSpan.FromMinutes(5);

        return teaRepository.GetTea(id);
    });

    if (tea == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(tea);
}).CacheOutput(o => o
.Tag(CacheTags.TEA)
.SetVaryByRouteValue("id")
.Expire(TimeSpan.FromMinutes(2)));

app.MapPost("CreateTea", async (TeaRepository teaRepository, [FromBody] Tea tea, IOutputCacheStore outputCache,
   IMemoryCache memoryCache) =>
{
    teaRepository.CreateTea(tea);

    memoryCache.Remove(CacheKeys.TEAS);
    await outputCache.EvictByTagAsync(CacheTags.TEAS, default);

    return Results.Ok(tea);
});

app.MapPut("ChangeDrink/{id}",
   async (TeaRepository teaRepository,
   int id, [FromBody] Tea tea,
   IOutputCacheStore outputCache,
   IMemoryCache memoryCache) =>
{
    var changedTea = teaRepository.ChangeTea(id, tea);

    if (changedTea == null)
    {
        return Results.NotFound();
    }

    memoryCache.Remove(CacheKeys.TEAS);
    memoryCache.Remove($"{CacheKeys.TEA}:{id}");
    await outputCache.EvictByTagAsync(CacheTags.TEAS, default);

    return Results.Ok(changedTea);
});

app.MapDelete("DeleteDrink",
    async (TeaRepository teaRepository,
    [FromBody] int id,
    IOutputCacheStore outputCache,
    IMemoryCache memoryCache) =>
{
    var canDelete = teaRepository.DeleteTea(id);
    if (!canDelete)
    {
        return false;
    }

    memoryCache.Remove(CacheKeys.TEAS);
    memoryCache.Remove($"{CacheKeys.TEA}:{id}");
    await outputCache.EvictByTagAsync(CacheTags.TEAS, default);

    return true;
});

app.MapGet("Exception", () => { throw new Exception(); });

// When Redis is on
app.MapGet("redis-test", async (IDistributedCache cache) =>
{
    var value = await cache.GetStringAsync("test");

    if (value == null)
    {
        // TTL
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


app.MapGet("GetTeasRedis", (TeaCacheService teaService) =>
{
    var teas = teaService.GetTeas();

    return Results.Ok(teas);
}).CacheOutput(o => o.Tag(CacheTags.TEAS));

app.MapGet("GetTeaRedis/{id}", (TeaCacheService teaService, int id) =>
{
    var tea = teaService.GetTea(id);

    return Results.Ok(tea);
}).CacheOutput(o => o.Tag(CacheTags.TEA)
.SetVaryByRouteValue("id")
.Expire(TimeSpan.FromMinutes(2)));

app.MapPost("CreateTeaRedis", async (TeaCacheService teaService, [FromBody] Tea tea, IOutputCacheStore outputCache) =>
{
    teaService.CreateTea(tea);

    await outputCache.EvictByTagAsync(CacheTags.TEAS, default);

    return Results.Ok(tea);
});

app.MapPut("ChangeDrinkRedis/{id}",
   async (TeaCacheService teaService,
   int id, [FromBody] Tea tea,
   IOutputCacheStore outputCache) =>
   {
       var changedTea = teaService.ChangeTea(id, tea);

       if (changedTea == null)
       {
           return Results.NotFound();
       }

       await outputCache.EvictByTagAsync(CacheTags.TEAS, default);

       return Results.Ok(changedTea);
   });

app.MapDelete("DeleteDrinkRedis",
    async (TeaCacheService teaService,
    [FromBody] int id,
    IOutputCacheStore outputCache) =>
    {
        var canDelete = teaService.DeleteTea(id);
        if (!canDelete)
        {
            return false;
        }

        await outputCache.EvictByTagAsync(CacheTags.TEAS, default);

        return true;
    });

app.Run();
