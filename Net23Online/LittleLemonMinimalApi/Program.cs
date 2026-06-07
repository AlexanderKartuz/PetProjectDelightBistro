using LittleLemonMinimalApi.DbStuff;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WebNet23LittleLemon;Integrated Security=True;Connect Timeout=30;";
builder.Services.AddDbContext<MiniDbContext>(op => op.UseSqlServer(connectionString));

var app = builder.Build();

app.UseCors();

app.MapGet("/", () => "Hello World!");

app.MapGet("GetMenuItems", (MiniDbContext dbContext, string? category) =>
{
    var allMenuItems = dbContext.MenuItems.ToList();

    if (string.IsNullOrEmpty(category))
    {
        return allMenuItems;
    }

    var filteredMenuItems = allMenuItems
        .Where(item => item.Category == category)
        .ToList();

    return filteredMenuItems.Count == 0
        ? allMenuItems
        : filteredMenuItems;
});

app.MapPost("CreateMenuItem", (MiniDbContext dbContext, [FromBody] MenuItem menuItem) =>
{
    dbContext.MenuItems.Add(menuItem);
    dbContext.SaveChanges();
    return menuItem;
});

app.MapDelete("DeleteMenuItem", (MiniDbContext dbContext, [FromBody] int id) =>
{
    var menuItem = dbContext.MenuItems.First(m => m.Id == id);
    dbContext.MenuItems.Remove(menuItem);
    dbContext.SaveChanges();
    return true;
});

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
