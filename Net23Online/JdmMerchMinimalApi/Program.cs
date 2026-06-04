using JdmMerchMinimalApi.DbConnection;
using JdmMerchMinimalApi.Models;
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

var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WebNet23JdmMerch;Integrated Security=True;Connect Timeout=30;";
builder.Services.AddDbContext<JdmMerchContext>(op => op.UseSqlServer(connectionString));

var app = builder.Build();

app.UseCors();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapGet("/GetMerches", (JdmMerchContext dbContext) =>
{
    return dbContext.jdmMerches.ToList();
});

app.MapPost("/CreateMerche", (JdmMerchContext dbContext, [FromBody] JdmMerchModel jdmMerch) =>
{
    dbContext.jdmMerches.Add(jdmMerch);
    dbContext.SaveChanges();
    return Results.Created($"/CreateMerche/{jdmMerch.Id}", jdmMerch);
});

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
