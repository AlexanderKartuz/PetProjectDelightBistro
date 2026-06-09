using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlayTheSpire2RelicsMinimalApi.DbStuff;



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

var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WebNet23SlayTheSpire2Relic;Integrated Security=True;Connect Timeout=30;";
builder.Services.AddDbContext<MiniDbContextSlayTheSpire2Relics>(op => op.UseSqlServer(connectionString));

var app = builder.Build();

app.UseCors();

app.MapGet("/", () => "Hello World!");

app.MapGet("GetRelics", (MiniDbContextSlayTheSpire2Relics DbContext) => DbContext.Relics.ToList());

app.MapPost("CreatRelic", (MiniDbContextSlayTheSpire2Relics dbContext, [FromBody] Relic relic) =>
{
    dbContext.Relics.Add(relic);
    dbContext.SaveChanges();
    return relic;
});

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
