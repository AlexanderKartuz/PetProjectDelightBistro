using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuotesMinimalApi.DbStuff;

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

var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WebNet23Quote;Integrated Security=True;Connect Timeout=30;";
builder.Services.AddDbContext<MiniDbContext>(op => op.UseSqlServer(connectionString));

var app = builder.Build();

app.UseCors();

app.MapGet("/", () => "Hello World!");

app.MapGet("GetQuotes", (MiniDbContext dbContext) => dbContext.Quotes.ToList());

app.MapPost("CreateQuote", (MiniDbContext dbContext, [FromBody] Quote quote) =>
{
    dbContext.Quotes.Add(quote);
    dbContext.SaveChanges();
    return quote;
});

app.UseSwagger();
app.UseSwaggerUI();


app.Run();
