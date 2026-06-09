using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieMinimalApi.DbStuff;
using MovieMinimalApi.Dtos;
using MovieMinimalApi.Services;

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

var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WebNet23Movie;Integrated Security=True;Connect Timeout=30;";
builder.Services.AddDbContext<MiniDbContext>(op => op.UseSqlServer(connectionString));
builder.Services.AddScoped<TagService>();

var app = builder.Build();

app.UseCors();

app.MapGet("/", () => "Hello World!");

app.MapGet("GetMovies", async (MiniDbContext dbContext, TagService tagService, [FromQuery] string? tag) =>
{
    var query = dbContext.Movies.Include(m => m.Tags).AsQueryable();

    if (!string.IsNullOrWhiteSpace(tag))
    {
        var normalized = tagService.Normalize(tag);
        query = query.Where(m => m.Tags.Any(t => t.Name.ToLower() == normalized.ToLower()));
    }

    var movies = await query.ToListAsync();
    return movies.Select(tagService.ToDto).ToList();
});

app.MapGet("GetMovie", async (MiniDbContext dbContext, TagService tagService, [FromQuery] int id) =>
{
    var movie = await dbContext.Movies
        .Include(m => m.Tags)
        .FirstOrDefaultAsync(m => m.Id == id);

    return movie is null ? Results.NotFound() : Results.Ok(tagService.ToDto(movie));
});

app.MapPost("CreateMovie", async (MiniDbContext dbContext, TagService tagService, [FromBody] CreateMovieRequest request) =>
{
    var movie = new Movie
    {
        Name = request.Name,
        Url = request.Url,
        Rating = request.Rating,
    };

    if (request.Tags is not null)
    {
        foreach (var tagName in request.Tags.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                continue;
            }

            var tag = await tagService.FindOrCreateTagAsync(tagName);
            movie.Tags.Add(tag);
        }
    }

    dbContext.Movies.Add(movie);
    await dbContext.SaveChangesAsync();

    await dbContext.Entry(movie).Collection(m => m.Tags).LoadAsync();
    return tagService.ToDto(movie);
});

app.MapPost("AddMovieTag", async (MiniDbContext dbContext, TagService tagService, [FromBody] TagRequest request) =>
{
    var movie = await dbContext.Movies
        .Include(m => m.Tags)
        .FirstOrDefaultAsync(m => m.Id == request.MovieId);

    if (movie is null)
    {
        return Results.NotFound();
    }

    var tag = await tagService.FindOrCreateTagAsync(request.TagName);

    if (movie.Tags.All(t => t.Id != tag.Id))
    {
        movie.Tags.Add(tag);
        await dbContext.SaveChangesAsync();
    }

    return Results.Ok(tagService.ToDto(movie));
});

app.MapDelete("RemoveMovieTag", async (MiniDbContext dbContext, TagService tagService, [FromBody] TagRequest request) =>
{
    var movie = await dbContext.Movies
        .Include(m => m.Tags)
        .FirstOrDefaultAsync(m => m.Id == request.MovieId);

    if (movie is null)
    {
        return Results.NotFound();
    }

    var normalized = tagService.Normalize(request.TagName);
    var tag = movie.Tags.FirstOrDefault(t => t.Name.ToLower() == normalized.ToLower());

    if (tag is not null)
    {
        movie.Tags.Remove(tag);
        await dbContext.SaveChangesAsync();
    }

    return Results.Ok(tagService.ToDto(movie));
});

app.MapDelete("DeleteMovie", async (MiniDbContext dbContext, [FromBody] int id) =>
{
    var movie = await dbContext.Movies.FirstOrDefaultAsync(m => m.Id == id);

    if (movie is null)
    {
        return Results.NotFound();
    }

    dbContext.Movies.Remove(movie);
    await dbContext.SaveChangesAsync();
    return Results.Ok(true);
});

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
