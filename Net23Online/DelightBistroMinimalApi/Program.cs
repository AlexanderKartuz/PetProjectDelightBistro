using DelightBistroMinimalApi.DbStuff;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DelightBistroMinimalApi.Middlewares;
using Microsoft.AspNetCore.OutputCaching;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WebNet23Tea;Integrated Security=True;Connect Timeout=30;";
builder.Services.AddDbContext<MiniDbContext>(op => op.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Output Caching, Запрос доходит до сервера, но эндпоинт не выполняется
// (вместо Response Caching Middleware)
builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(30);
    options.MaximumBodySize = 64 * 1024;// макс размер ответа
    options.SizeLimit = 100 * 1024 * 1024; //общий размер кеша
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


//MemoryCache / IMemoryCache (кеш в коде)
//Запрос доходит до эндпоинта, но БД не вызывается



app.UseCustomExeptionHandling();
app.UseOutputCache(); // middleware
app.UseResponseHeader();// заголовки, кеш в браузере headers
app.UseCustomRequestLogging();


app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello World!");

app.MapGet("GetTeas", (MiniDbContext dbContext) =>
{
    Console.WriteLine("Иду в базу");
    var teas = dbContext.Teas.ToList();
    return Results.Ok(teas);
}
).CacheOutput(o => o.Tag("teas")); // не дойдет до эндпоинта
                                       // , middleware сразу выдаст ответ при совпадении запросов?

app.MapGet("GetTea/{id}", (MiniDbContext dbContext, int id) =>
{
    Console.WriteLine("Иду в базу");
    var tea = dbContext.Teas.FirstOrDefault(t => t.Id == id);

    if (tea == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(tea);
}).CacheOutput(o => o.Expire(TimeSpan.FromMinutes(3)));

app.MapPost("CreateTea",
   async (MiniDbContext dbContext, [FromBody] Tea tea, IOutputCacheStore cache) =>
{
    dbContext.Teas.Add(tea);
    dbContext.SaveChanges();

    await cache.EvictByTagAsync("teas", default); //При создании сбрасываем кеш сервера

    return tea;
});

app.MapPut("ChangeDrink/{id}",
   async (MiniDbContext dbContext, int id, [FromBody] Tea tea, IOutputCacheStore cache) =>
{
    var changedTea = dbContext.Teas.FirstOrDefault(t => t.Id == id);

    if (changedTea == null)
    {
        return Results.NotFound();
    }

    changedTea.Name = tea.Name;
    changedTea.Price = tea.Price;

    dbContext.SaveChanges();

    await cache.EvictByTagAsync("teas", default);

    return Results.Ok(changedTea);
});

app.MapDelete("DeleteDrink",
    async (MiniDbContext dbContext, [FromBody] int id, IOutputCacheStore cache) =>
{
    var tea = dbContext.Teas.First(i => i.Id == id);
    dbContext.Teas.Remove(tea);
    dbContext.SaveChanges();

    await cache.EvictByTagAsync("teas", default);

    return true;
});

app.MapGet("Exception", () => { throw new Exception(); });

app.Run();
