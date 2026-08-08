using Microsoft.EntityFrameworkCore;
using Quartz;
using WebNet23Online.Data;
using WebNet23Online.Hubs;
using WebNet23Online.MiddlewareServices;
using WebNet23Online.RelfectionTools;
using WebNet23Online.Services;
using WebNet23Online.Services.Apis;
using WebNet23Online.Services.BackgroundServices;
using WebNet23Online.Services.DelightBistro;
using WebNet23Online.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WebNet23Online;Integrated Security=True;Connect Timeout=30;";
builder.Services.AddDbContext<WebContext>(op => op.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services
    .AddAuthentication(AuthService.AUTH_KEY)
    .AddCookie(AuthService.AUTH_KEY, option =>
    {
        option.LoginPath = "/Auth/Login";
        option.AccessDeniedPath = "/Auth/Deny";
        option.ExpireTimeSpan = TimeSpan.FromHours(2);
    });


builder.Services.AddHttpClient<JokeApi>(x =>
{
    x.BaseAddress = new Uri("https://official-joke-api.appspot.com");
});

builder.Services.AddHttpClient<CatFactApi>(x =>
x.BaseAddress = new Uri("https://catfact.ninja"));

builder.Services.AddHttpClient<DogApi>(x =>
x.BaseAddress = new Uri("https://dog.ceo"));

//DelightBistro DI
builder.Services.AddScoped<IFoodItemGenerator, FoodItemGenerator>();
builder.Services.AddScoped<IMenuTypeGenerator, MenuTypeGenerator>();
builder.Services.AddScoped<IIngredientGenerator, IngredientGenerator>();
builder.Services.AddScoped<IDelightBistroMainIndexGenerator, DelightBistroMainIndexGenerator>();

// Repositories
builder.Services.ResolveRepositories();
builder.Services.ResolveByAttribute();


builder.Services.AddHostedService<NotificationBackgroundService>();

builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});
builder.Services.AddHostedService<DelightBistroOrderBackgroundService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(p =>
    {
        p.AllowAnyHeader();
        p.AllowAnyMethod();
        p.SetIsOriginAllowed(_ => true);
        p.AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthentication();    
app.UseAuthorization();     

app.UseMiddleware<MyLocalizationMiddleware>();

app.MapHub<DeligtBistroHub>("/my-hub/delightbistro");
app.MapHub<NotificationHub>("/my-hub/notification");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=DelightBistro}/{action=Index}/{id?}");

app.Run();
