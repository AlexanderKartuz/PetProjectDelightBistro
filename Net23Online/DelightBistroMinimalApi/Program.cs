using DelightBistroMinimalApi.Constans;
using DelightBistroMinimalApi.DbStuff;
using DelightBistroMinimalApi.Middlewares;
using DelightBistroMinimalApi.Middlewares.RateLimit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WebNet23Tea;Integrated Security=True;Connect Timeout=30;";
builder.Services.AddDbContext<MiniDbContext>(op => op.UseSqlServer(connectionString));
builder.Services.AddScoped<TeaService>();
builder.Services.AddScoped<TeaRepository>();


builder.Services.AddCustomRateLimiter(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Кеш HTTP ответов
builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(60);
    options.MaximumBodySize = 64 * 1024;// макс размер ответа
    options.SizeLimit = 100 * 1024 * 1024; //общий размер кеша
});

// TO DO: ADD TTL
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "delightBistro";
});


// Кеш данных в endpoints
//Запрос доходит до эндпоинта, но БД не вызывается
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
app.UseOutputCache(); // Запрос доходит до сервера, но эндпоинт не выполняется
app.UseCustomRequestLogging();


app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello World!");

app.MapGet("GetTeas", /*async*/ (TeaRepository teaRepository, IMemoryCache memoryCache/*, CancellationToken cancellationToken*/) =>
{
    var teas = /*await*/ memoryCache.GetOrCreate/*GetOrCreateAsync*/(CacheKeys.TEAS, /*async*/ entry =>
    {
        entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(5);
        entry.SlidingExpiration = TimeSpan.FromMinutes(2);

        return teaRepository.GetTeas();/*ToListAsync(cancellationToken);*/ // cancellationToken only in async
    });

    return Results.Ok(teas);
}).CacheOutput(o => o.Tag(CacheTags.TEAS));


app.MapGet("GetTea/{id}", (TeaRepository teaRepository, int id, IMemoryCache memoryCache) =>
{
    var cacheKey = $"{CacheKeys.TEA}:{id}";

    var tea = memoryCache.GetOrCreate(cacheKey, entry =>
    {
        entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(10); // Истекает в теч 10минут после использования 
        entry.SlidingExpiration = TimeSpan.FromMinutes(5); // Истекает после не использования в теч 5 минут

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


app.MapGet("GetTeasRedis", (TeaService teaService) =>
{
    var teas = teaService.GetTeas();

    return Results.Ok(teas);
}).CacheOutput(o => o.Tag(CacheTags.TEAS));

app.MapGet("GetTeaRedis/{id}", (TeaService teaService, int id) =>
{
    var tea = teaService.GetTea(id);

    return Results.Ok(tea);
}).CacheOutput(o => o.Tag(CacheTags.TEA)
.SetVaryByRouteValue("id")
.Expire(TimeSpan.FromMinutes(2)));

app.MapPost("CreateTeaRedis", async (TeaService teaService, [FromBody] Tea tea, IOutputCacheStore outputCache) =>
{
    teaService.CreateTea(tea);

    await outputCache.EvictByTagAsync(CacheTags.TEAS, default);

    return Results.Ok(tea);
});

app.MapPut("ChangeDrinkRedis/{id}",
   async (TeaService teaService,
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
    async (TeaService teaService,
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
