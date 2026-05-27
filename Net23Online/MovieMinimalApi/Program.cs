using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieMinimalApi.DbStuff;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(p =>
    {
        p.AllowAnyHeader();
        p.AllowAnyMethod();
        // p.AllowAnyOrigin();
        p.SetIsOriginAllowed(x => true);
        p.AllowCredentials();
    });
});

var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WebNet23Movie;Integrated Security=True;Connect Timeout=30;";
builder.Services.AddDbContext<MiniDbContext>(op => op.UseSqlServer(connectionString));

var app = builder.Build();

app.UseCors();

app.MapGet("/", () => "Hello World!");

app.MapGet("GetMovies", (MiniDbContext dbContext) => dbContext.Movies.ToList());

app.MapPost("CreateMovie", (MiniDbContext dbContext, [FromBody]Movie movie) =>
{
    dbContext.Movies.Add(movie);
    dbContext.SaveChanges();
    return movie;
});

app.UseSwagger();
app.UseSwaggerUI();


app.Run();
