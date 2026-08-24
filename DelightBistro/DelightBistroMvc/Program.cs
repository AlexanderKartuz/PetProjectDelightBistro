using DelightBistroMvc.Data;
using DelightBistroMvc.Data.Repositories;
using DelightBistroMvc.Data.Repositories.Interfaces;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Data.Services.PasswordHasher;
using DelightBistroMvc.Data.Services.UserService;
using DelightBistroMvc.Hubs;
using DelightBistroMvc.MiddlewareServices;
using DelightBistroMvc.RelfectionTools;
using DelightBistroMvc.Services;
using DelightBistroMvc.Services.Apis;
using DelightBistroMvc.Services.BackgroundServices;
using DelightBistroMvc.Services.Chat;
using DelightBistroMvc.Services.Chat.Interfaces;
using DelightBistroMvc.Services.DelightBistro;
using DelightBistroMvc.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Quartz;


var builder = WebApplication.CreateBuilder(args);


var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WebNet23Online;Integrated Security=True;Connect Timeout=30;";
builder.Services.AddDbContext<WebContext>(op => op.UseSqlServer(connectionString));

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUserDataService, UserDataService>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services
    .AddAuthentication(AuthService.AUTH_KEY)
    .AddCookie(AuthService.AUTH_KEY, option =>
    {
        option.LoginPath = "/Auth/Login";
        option.AccessDeniedPath = "/Auth/Deny";
        option.ExpireTimeSpan = TimeSpan.FromHours(2);

        option.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        option.Cookie.HttpOnly = true;
        option.SlidingExpiration = true; // Продлить срок жизни при активности
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
builder.Services.AddScoped<IAuthService, AuthService>();

// Chat
builder.Services.AddSignalR();
builder.Services.AddSingleton<ChatPresenceService>();
builder.Services.AddScoped<INewChatService, NewChatService>();

// DataSeed
builder.Services.AddScoped<IDelightBistroSeedService, DelightBistroSeedService>();

// Repositories
//builder.Services.ResolveRepositories();
//builder.Services.ResolveByAttribute();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFoodItemRepository, FoodItemRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IIngredientsRepository, IngredientsRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();


builder.Services.AddHostedService<NotificationBackgroundService>();
builder.Services.AddHostedService<DelightBistroOrderBackgroundService>();

builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});

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

// DataSeed
using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider
         .GetRequiredService<IDelightBistroSeedService>()
         .EnsureSeedAsync();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<MyLocalizationMiddleware>();

// hubs
app.MapHub<DeligtBistroHub>("/my-hub/delightbistro");
app.MapHub<NotificationHub>("/my-hub/notification");
app.MapHub<NewChatHub>("/my-hub/new-chat");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=DelightBistro}/{action=Index}/{id?}");

app.Run();
