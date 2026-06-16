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

app.MapGet("GetTeas", async (MiniDbContext dbContext, IMemoryCache memoryCache, CancellationToken cancellationToken) =>
{
    var teas = await memoryCache.GetOrCreateAsync(CacheKeys.TEAS, async entry =>
    {
        entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(5);
        entry.SlidingExpiration = TimeSpan.FromMinutes(2);

        Console.WriteLine("Иду в базу");
        return await dbContext.Teas.ToListAsync(cancellationToken); // cancellationToken only in async
    });

    return Results.Ok(teas);
}).CacheOutput(o => o.Tag(CacheTags.TEAS));


app.MapGet("GetTea/{id}", (MiniDbContext dbContext, int id, IMemoryCache memoryCache) =>
{
    var cacheKey = $"{CacheKeys.TEA}:{id}";

    var tea = memoryCache.GetOrCreate(cacheKey, entry =>
    {
        entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(10); // Истекает в теч 10минут после использования 
        entry.SlidingExpiration = TimeSpan.FromMinutes(5); // Истекает после не использования в теч 5 минут

        return dbContext.Teas.FirstOrDefault(t => t.Id == id);
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

app.MapPost("CreateTea", async (MiniDbContext dbContext, [FromBody] Tea tea, IOutputCacheStore outputCache,
   IMemoryCache memoryCache) =>
{
    dbContext.Teas.Add(tea);
    dbContext.SaveChanges();
    // Сброс ключа или тега
    memoryCache.Remove(CacheKeys.TEAS);
    await outputCache.EvictByTagAsync(CacheTags.TEAS, default);

    return Results.Ok(tea);
});

app.MapPut("ChangeDrink/{id}",
   async (MiniDbContext dbContext,
   int id, [FromBody] Tea tea,
   IOutputCacheStore outputCache,
   IMemoryCache memoryCache) =>
{
    var changedTea = dbContext.Teas.FirstOrDefault(t => t.Id == id);

    if (changedTea == null)
    {
        return Results.NotFound();
    }

    changedTea.Name = tea.Name;
    changedTea.Price = tea.Price;
    changedTea.Description = tea.Description;
    changedTea.ImgUrl = tea.ImgUrl;

    dbContext.SaveChanges();

    memoryCache.Remove(CacheKeys.TEAS);
    memoryCache.Remove($"{CacheKeys.TEA}:{id}");
    await outputCache.EvictByTagAsync(CacheTags.TEAS, default);

    return Results.Ok(changedTea);
});

app.MapDelete("DeleteDrink",
    async (MiniDbContext dbContext,
    [FromBody] int id,
    IOutputCacheStore outputCache,
    IMemoryCache memoryCache) =>
{
    var tea = dbContext.Teas.FirstOrDefault(i => i.Id == id);

    if (tea == null)
    {
        return false;
    }

    dbContext.Teas.Remove(tea);
    dbContext.SaveChanges();

    memoryCache.Remove(CacheKeys.TEAS);
    memoryCache.Remove($"{CacheKeys.TEA}:{id}");
    await outputCache.EvictByTagAsync(CacheTags.TEAS, default);

    return true;
});

app.MapGet("Exception", () => { throw new Exception(); });

app.MapGet("/redis-test", async (IDistributedCache cache) =>
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


app.Run();
