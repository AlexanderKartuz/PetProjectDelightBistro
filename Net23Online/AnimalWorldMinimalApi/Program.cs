using AnimalWorldMinimalApi.DbStuff;
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

var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WebNet23AnimalFacts;Integrated Security=True;Connect Timeout=30;";
builder.Services.AddDbContext<AnimalWorldDbContext>(op => op.UseSqlServer(connectionString));

var app = builder.Build();

app.UseCors();

app.MapGet("/", () => "Hello World!");

app.MapGet("GetFacts", (AnimalWorldDbContext dbContext) => dbContext.InterestingFacts.ToList());

app.MapPost("AddFact", (AnimalWorldDbContext dbContext, [FromBody]InterestingFact fact) =>
{
    dbContext.InterestingFacts.Add(fact);
    dbContext.SaveChanges();
    return fact;
});

app.UseSwagger();
app.UseSwaggerUI();

app.Run();