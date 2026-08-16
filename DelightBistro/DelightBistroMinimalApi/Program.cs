using DelightBistro.Services.Logging;
using DelightBistroMinimalApi.Constans;
using DelightBistroMinimalApi.DbStuff;
using DelightBistroMinimalApi.Mappings;
using DelightBistroMinimalApi.Middlewares;
using DelightBistroMinimalApi.Middlewares.RateLimit;
using DelightBistroMinimalApi.ModelsDto;
using DelightBistroMinimalApi.ModelsDto.EntityDto;
using DelightBistroMinimalApi.Services.Auth;
using DelightBistroMinimalApi.Services.Auth.Interfaces;
using DelightBistroMinimalApi.Services.Auth.Options;
using DelightBistroMinimalApi.Services.Cache;
using DelightBistroMinimalApi.Services.Cache.Interfaces;
using DelightBistroMinimalApi.Validation;
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
builder.Services.AddScoped<IDrinkMapper, DrinkMapper>();
builder.Services.AddScoped<IEndpointValidator, EndpointValidator>();

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
    IEndpointValidator endpointValidator,
    IJwtTokenService jwtTokenService,
    IOptions<JwtOptions> options) =>
{
    var validationError = endpointValidator.Validate(request);
    if (validationError != null)
    {
        return validationError;
    }

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


app.MapGet("GetDrinks", async (IDrinksCacheService drinksCache, IDrinkMapper drinkMapper) =>
{
    var drinks = await drinksCache.GetDrinksAsync();
    var drinksResponse = drinkMapper.ToDrinkListResponse(drinks);
    return Results.Ok(drinksResponse);

}).CacheOutput(o => o.Tag(CacheTags.DRINKS));

app.MapGet("GetDrink/{id}",
    async (IDrinksCacheService drinksCache, int id, IDrinkMapper drinkMapper) =>
{
    var drink = await drinksCache.GetDrinkAsync(id);

    if (drink == null)
    {
        return Results.NotFound();
    }

    var drinkResponse = drinkMapper.ToDrinkResponse(drink);

    return Results.Ok(drinkResponse);

}).CacheOutput(o => o
.Tag(CacheTags.DRINK)
.SetVaryByRouteValue("id")
.Expire(TimeSpan.FromMinutes(1)));

app.MapPost("CreateDrink",
    async (IDrinksCacheService drinksCache,
    [FromBody] DrinkRequest drinkRequiest,
    IOutputCacheStore outputCache,
    IDrinkMapper drinkMapper,
    IEndpointValidator endpointValidator) =>
{
    var validationError = endpointValidator.Validate(drinkRequiest);
    if (validationError != null)
    {
        return validationError;
    }

    var drinkData = drinkMapper.ToEntity(drinkRequiest);

    var task1 = drinksCache.CreateDrinkAsync(drinkData);
    var task2 = outputCache.EvictByTagAsync(CacheTags.DRINKS, default).AsTask();
    await Task.WhenAll(task1, task2);

    var drinkResponse = drinkMapper.ToDrinkResponse(drinkData);

    return Results.Ok(drinkResponse);
});

app.MapPut("ChangeDrink/{id}",
   async (IDrinksCacheService drinksCache,
   int id, [FromBody] DrinkRequest drinkRequiest,
   IOutputCacheStore outputCache,
    IDrinkMapper drinkMapper,
    IEndpointValidator endpointValidator) =>
{
    var validationError = endpointValidator.Validate(drinkRequiest);
    if (validationError != null)
    {
        return validationError;
    }

    var drinkData = drinkMapper.ToEntity(drinkRequiest);

    var changedDrink = await drinksCache.ChangeDrinkAsync(id, drinkData);

    if (changedDrink == null)
    {
        return Results.NotFound();
    }

    // from SetVaryByRouteValue
    var cacheKey = $"GET:/ChangeDrink/{id}:routeId={id}";
    var task1 = outputCache.EvictByTagAsync(cacheKey, default).AsTask();
    var task2 = outputCache.EvictByTagAsync(CacheTags.DRINKS, default).AsTask();
    await Task.WhenAll(task1, task2);

    var drinkResponse = drinkMapper.ToDrinkResponse(changedDrink);


    return Results.Ok(drinkResponse);
});

app.MapDelete("DeleteDrink/{id}",
    async (
        int id,
        IDrinksCacheService drinksCache,
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
