using DelightBistro.Services.Logging;
using DelightBistroMinimalApi.Constans;
using DelightBistroMinimalApi.DbStuff;
using DelightBistroMinimalApi.Middlewares;
using DelightBistroMinimalApi.Middlewares.RateLimit;
using DelightBistroMinimalApi.ModelsDto;
using DelightBistroMinimalApi.Services.Auth;
using DelightBistroMinimalApi.Services.Auth.Interfaces;
using DelightBistroMinimalApi.Services.Auth.Options;
using DelightBistroMinimalApi.Services.Cache;
using DelightBistroMinimalApi.Services.Cache.Interfaces;
using DelightBistroMvc.Data.Enums;
using DelightBistroMvc.Data.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

//service and appsetings
var connectionString = builder.Configuration.GetConnectionString("Drinks");
builder.Services.AddDbContext<MiniDbContext>(op => op.UseSqlServer(connectionString));
builder.Services.AddScoped<IDrinkRepository, DrinkRepository>();


var cachingOptions = builder.Services.AddDelightBistroCaching(builder.Configuration);

builder.Services.AddScoped(typeof(IAppLogging<>), typeof(AppLogging<>));
builder.ConfigureSeriLog();

builder.AddCustomRateLimiter();

builder.Services.AddDelightBistroJwtAuth(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDelightBistroSwaggerWithJwt();

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

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.UseResponseHeader();
app.UseOutputCache();
app.UseCustomRequestLogging();

app.UseSwagger();
app.UseSwaggerUI();

//service

app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapPost("login", (
    LoginRequest request,
    IUserDataService userDataService,
    IJwtTokenService jwtTokenService,
    IOptions<JwtOptions> options) =>
{
    var user = userDataService.ValidateCredetials(request.Login, request.Password);
    if (user is null)
    {
        return Results.Unauthorized();
    }

    var token = jwtTokenService.CreateToken(user);
    return Results.Ok(new LoginResponse
    {
        AccessToken = token,
        ExpiresInMinutes = options.Value.ExpireMinutes,
    });
});


app.MapGet("GetDrinks", async (IDrinksCacheService drinksCache) =>
{
    var drinks = await drinksCache.GetDrinksAsync();

    return Results.Ok(drinks);

}).CacheOutput(o => o.Tag(CacheTags.DRINKS));

app.MapGet("GetDrink/{id}",
    async (IDrinksCacheService drinksCache, int id) =>
{
    var drink = await drinksCache.GetDrinkAsync(id);

    if (drink == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(drink);

}).CacheOutput(o => o
.Tag(CacheTags.DRINK)
.SetVaryByRouteValue("id")
.Expire(TimeSpan.FromMinutes(1)));

app.MapPost("CreateDrink",
    async (IDrinksCacheService drinksCache,
    [FromBody] Drink drink,
    IOutputCacheStore outputCache) =>
{
    var task1 = drinksCache.CreateDrinkAsync(drink);
    var task2 = outputCache.EvictByTagAsync(CacheTags.DRINKS, default).AsTask();
    await Task.WhenAll(task1, task2);

    return Results.Ok(drink);
});

app.MapPut("ChangeDrink/{id}",
   async (IDrinksCacheService drinksCache,
   int id, [FromBody] Drink drink,
   IOutputCacheStore outputCache) =>
{
    var changedDrink = await drinksCache.ChangeDrinkAsync(id, drink);

    if (changedDrink == null)
    {
        return Results.NotFound();
    }

    // from SetVaryByRouteValue
    var cacheKey = $"GET:/ChangeDrink/{id}:routeId={id}";
    var task1 = outputCache.EvictByTagAsync(cacheKey, default).AsTask();
    var task2 = outputCache.EvictByTagAsync(CacheTags.DRINKS, default).AsTask();
    await Task.WhenAll(task1, task2);

    return Results.Ok(changedDrink);
});

app.MapDelete("DeleteDrink",
    async (IDrinksCacheService drinksCache,
    [FromBody] int id,
    IOutputCacheStore outputCache) =>
{
    var canDelete = await drinksCache.DeleteDrinkAsync(id);
    if (!canDelete)
    {
        return Results.NotFound();
    }

    var cacheKey = $"GET:/DeleteDrink/{id}:routeId={id}";

    var task1 = outputCache.EvictByTagAsync(cacheKey, default).AsTask();
    var task2 = outputCache.EvictByTagAsync(CacheTags.DRINKS, default).AsTask();
    await Task.WhenAll(task1, task2);
    await Task.WhenAll(task1, task2);
    return Results.NoContent();
})
    .RequireAuthorization(policy => policy.RequireRole(nameof(UserRole.Admin)));

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
}

app.Run();
