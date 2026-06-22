using MazeCore;
using MazeCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebNet23Online.Data;
using WebNet23Online.Data.Repositories;
using WebNet23Online.Data.Repositories.AnimalWorld;
using WebNet23Online.Data.Repositories.Interfaces;
using WebNet23Online.Data.Repositories.Interfaces.AnimalWorld;
using WebNet23Online.Data.Repositories.Interfaces.DelightBistro;
using WebNet23Online.Data.Repositories.Interfaces.HabitTracker;
using WebNet23Online.Data.Repositories.Interfaces.Steam;
using WebNet23Online.Data.Repositories.Steam;
using WebNet23Online.Hubs;
using WebNet23Online.MiddlewareServices;
using WebNet23Online.RelfectionTools;
using WebNet23Online.Services;
using WebNet23Online.Services.Apis;
using WebNet23Online.Services.BackgroundServices;
using WebNet23Online.Services.Apis.steam;
using WebNet23Online.Services.DelightBistro;
using WebNet23Online.Services.Interfaces;
using WebNet23Online.Services.Interfaces.LittleLemon;
using WebNet23Online.Services.Interfaces.Steam;
using WebNet23Online.Services.LittleLemon;

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
        option.ExpireTimeSpan = TimeSpan.FromMinutes(13);
    });


builder.Services.AddScoped<ILittleLemonMenuService, LittleLemonMenuService>();
builder.Services.AddScoped<ILittleLemonTestimonialService, LittleLemonTestimonialService>();
builder.Services.AddScoped<ILittleLemonSubscribeService, LittleLemonSubscribeService>();
builder.Services.AddScoped<ILittleLemonReservationService, LittleLemonReservationService>();
builder.Services.AddScoped<ILittleLemonChatService, LittleLemonChatService>();

builder.Services.AddHttpClient<JokeApi>(x =>
{
    x.BaseAddress = new Uri("https://official-joke-api.appspot.com");
});

builder.Services.AddHttpClient<WaifuApi>(x =>
{
    x.BaseAddress = new Uri("https://api.waifu.im");
});

builder.Services.AddHttpClient<CatApi>(x =>
{
    x.BaseAddress = new Uri("https://cataas.com");
});

builder.Services.AddHttpClient<AnimalWorldRandomAnimalApi>(x =>
{
    x.BaseAddress = new Uri("https://api.some-random-api.com");
});

builder.Services.AddHttpClient<CatFactApi>(x =>
x.BaseAddress = new Uri("https://catfact.ninja"));

builder.Services.AddHttpClient<DogApi>(x =>
x.BaseAddress = new Uri("https://dog.ceo"));

builder.Services.AddHttpClient<RawgApi>(client =>
{
    client.BaseAddress = new Uri("https://api.rawg.io/api/");
});

builder.Services.AddHttpClient<RockApi>(x =>
{
    x.BaseAddress = new Uri("https://itunes.apple.com");
});

// Register Services
//builder.Services.AddScoped<IAnimeGirlGenerator, AnimeGirlGenerator>(diContainer =>
//{
//    var randomBuilderAAAA = diContainer.GetService<IRandomBuilder>(); // 24
//    var epicMeanlessPhraseGenerator = diContainer.GetService<IEpicMeanlessPhraseGenerator>(); // 17
//    var animeGirlGenerator = new AnimeGirlGenerator(epicMeanlessPhraseGenerator, randomBuilderAAAA);

//    return animeGirlGenerator;
//});
//builder.Services.AddScoped<IEpicMeanlessPhraseGenerator, EpicMeanlessPhraseGenerator>(diContainer =>
//{
//    var randomBuilderBBBB = diContainer.GetService<IRandomBuilder>(); // 24
//    var epicMeanlessPhraseGenerator = new EpicMeanlessPhraseGenerator(randomBuilderBBBB);
//    return epicMeanlessPhraseGenerator;
//});

//builder.Services.AddScoped<IRandomBuilder, RandomBuilder>(diContainer =>
//{
//    var randomBuilder = new RandomBuilder();
//    return randomBuilder;
//});
//builder.Services.AddTransient<IRandomBuilder, RandomBuilder>(); // One for each Call
//builder.Services.AddScoped<IRandomBuilder, RandomBuilder>();    // One for each Request (user)
//builder.Services.AddSingleton<IRandomBuilder, RandomBuilder>(); // One.

//             Life Time
// Transient < Scoped < Singleton

builder.Services.AddScoped<IAnimeGirlService, AnimeGirlGenerator>();
builder.Services.AddScoped<IEpicMeanlessPhraseGenerator, EpicMeanlessPhraseGenerator>();
//builder.Services.AddScoped<IRandomBuilder, RandomBuilder>();
//builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddSingleton<IMazeBuilder, MazeBuilder>();
builder.Services.AddSingleton<IMazeService, MazeService>();

//AnimalWorld DI
builder.Services.AddScoped<IAnimalWorldService, AnimalWorldService>();
builder.Services.AddScoped<IAnimalWorldMapper, AnimalWorldMapper>();

builder.Services.AddScoped<IRockBandsService, RockBandsService>();

builder.Services.AddSingleton<IRockLegendsPick, RockLegendsPick>();

builder.Services.AddSingleton<ISlayTheSpire2RewardImageService, SlayTheSpire2RewardImageService>();
builder.Services.AddSingleton<ISlayTheSpire2CardOptionsService, SlayTheSpire2CardOptionsService>();

builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<IChatService, ChatService>();

//DelightBistro DI
builder.Services.AddScoped<IFoodItemGenerator, FoodItemGenerator>();
builder.Services.AddScoped<IMenuTypeGenerator, MenuTypeGenerator>();
builder.Services.AddScoped<IIngredientGenerator, IngredientGenerator>();
builder.Services.AddScoped<IDelightBistroMainIndexGenerator, DelightBistroMainIndexGenerator>();

//HabitTracker DI
builder.Services.AddScoped<IHabitService, HabitService>();
builder.Services.AddScoped<IHabitStatisticsService, HabitStatisticsService>();

//JapaneseDomesticMarker DI
builder.Services.AddScoped<IJapaneseDomesticMarketGenerator, JapaneseDomesticMarketGenerator>();
builder.Services.AddScoped<IJDMCatalogGenerator, JDMCatalogGenerator>();

// Repositories
builder.Services.ResolveRepositories();
builder.Services.ResolveByAttribute();

builder.Services.AddScoped<ITicketService, TicketService>();

builder.Services.AddScoped<ICommentsService, CommentsService>();
builder.Services.AddScoped<ICommentsMapper, CommentMapper>();

builder.Services.AddHostedService<NotificationBackgroundService>();

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

app.UseAuthentication();    // Who Am I?
app.UseAuthorization();     // May I?

app.UseMiddleware<MyLocalizationMiddleware>();

app.MapHub<AnimeHub>("/my-hub/anime");
app.MapHub<DeligtBistroHub>("/my-hub/delightbistro");
app.MapHub<RockLegendsHub>("/my-hub/rock-legends");
app.MapHub<AnimalWorldHub>("/my-hub/animal-world");
app.MapHub<JdmHub>("/my-hub/jdm");
app.MapHub<SteamChatHub>("/steam/community-chat");
app.MapHub<SteamNotificationHub>("/steam/notification");
app.MapHub<LittleLemonHub>("/my-hub/little-lemon");
app.MapHub<NotificationHub>("/my-hub/notification");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
