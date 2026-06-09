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

app.MapPut("ChangeDrink/{id}", (MiniDbContext dbContext, int id, [FromBody] Tea tea) =>
{
    var changedTea = dbContext.Teas.FirstOrDefault(t => t.Id == id);

    if (changedTea == null)
    {
        return Results.NotFound();
    }

    changedTea.Name = tea.Name;
    changedTea.Price = tea.Price;

    dbContext.SaveChanges();

    return Results.Ok(changedTea);
});

app.MapDelete("DeleteDrink", (MiniDbContext dbContext, [FromBody] int id) =>
{
    var tea = dbContext.Teas.First(i => i.Id == id);
    dbContext.Teas.Remove(tea);
    dbContext.SaveChanges();
    return true;
});

app.Run();
