using DelightBistroMinimalApi.DbStuff;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WebNet23Tea;Integrated Security=True;Connect Timeout=30;";
builder.Services.AddDbContext<MiniDbContext>(op => op.UseSqlServer(connectionString));

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

var app = builder.Build();

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello World!");

app.MapGet("GetTeas", (MiniDbContext dbContext) => dbContext.Teas.ToList());

app.MapPost("CreateTea", (MiniDbContext dbContext, [FromBody] Tea tea) =>
{
    dbContext.Teas.Add(tea);
    dbContext.SaveChanges();

    return tea;
});

app.MapDelete("DeleteDrink", (MiniDbContext dbContext, [FromBody] int id) =>
{
    var drink = dbContext.Teas.First(i => i.Id == id);
    dbContext.Teas.Remove(drink);
    dbContext.SaveChanges();
    return true;
});

app.Run();
