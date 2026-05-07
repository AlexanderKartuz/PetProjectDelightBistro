using MazeCore;
using MazeCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebNet23Online.Data;
using WebNet23Online.Data.Repositories;
using WebNet23Online.Data.Repositories.AnimalWorld;
using WebNet23Online.Data.Repositories.Interfaces;
using WebNet23Online.Data.Repositories.Interfaces.AnimalWorld;
using WebNet23Online.Data.Repositories.Interfaces.DelightBistro;
using WebNet23Online.Data.Repositories.Interfaces.Steam;
using WebNet23Online.Data.Repositories.Steam;
using WebNet23Online.MiddlewareServices;
using WebNet23Online.Services;
using WebNet23Online.Services.DelightBistro;
using WebNet23Online.Services.Interfaces;
using WebNet23Online.Services.Interfaces.LittleLemon;
using WebNet23Online.Services.LittleLemon;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddScoped<IRandomBuilder, RandomBuilder>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddSingleton<IMazeBuilder, MazeBuilder>();
builder.Services.AddSingleton<IMazeService, MazeService>();

//AnimalWorld DI
builder.Services.AddScoped<IAnimalWorldService, AnimalWorldService>();
builder.Services.AddScoped<IAnimalWorldMapper, AnimalWorldMapper>();

builder.Services.AddScoped<IRockBandsService, RockBandsService>();

builder.Services.AddSingleton<IRockLegendsPick, RockLegendsPick>();

builder.Services.AddSingleton<ISlayTheSpire2RewardImageService, SlayTheSpire2RewardImageService>();

builder.Services.AddScoped<ICatalogService, CatalogService>();

//DelightBistro DI
builder.Services.AddScoped<IFoodItemGenerator, FoodItemGenerator>();
builder.Services.AddScoped<IMenuTypeGenerator, MenuTypeGenerator>();
builder.Services.AddScoped<IIngredientGenerator, IngredientGenerator>();


//HabitTracker DI
builder.Services.AddScoped<IHabitService, HabitService>();
builder.Services.AddScoped<IHabitStatisticsService, HabitStatisticsService>();


//JapaneseDomesticMarker DI
builder.Services.AddScoped<IJapaneseDomesticMarketGenerator, JapaneseDomesticMarketGenerator>();
builder.Services.AddScoped<IJDMCatalogGenerator, JDMCatalogGenerator>();

// Repositories
builder.Services.AddScoped<IZooRepository, ZooRepository>();
builder.Services.AddScoped<IAnimalFamilyRepository, AnimalFamilyRepository>();
builder.Services.AddScoped<IAnimalSpeciesRepository, AnimalSpeciesRepository>();
builder.Services.AddScoped<IAnimeGirlRepository, AnimeGirlRepository>();
builder.Services.AddScoped<IAnimeRepository, AnimeRepository>();
builder.Services.AddScoped<IMazeRepository, MazeRepository>();
builder.Services.AddScoped<IHabitRepository, HabitRepository>();
builder.Services.AddScoped<IHabitDoneDatesRepository, HabitDoneDatesRepository>();
builder.Services.AddScoped<IHabitDiaryRepository, HabitDiaryRepository>();
builder.Services.AddScoped<ISlayTheSpire2HeroesRepository, SlayTheSpire2HeroesRepository>();
builder.Services.AddScoped<IRockLegendsRepository, RockLegendsRepository>();
builder.Services.AddScoped<IFoodItemRepository, FoodItemRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IIngredientsRepository, IngredientsRepository>();
builder.Services.AddScoped<IRockBandsRepository, RockBandsRepository>();
builder.Services.AddScoped<IGenreOfRockBandsRepository, GenreOfRockBandsRepository>();
builder.Services.AddScoped<ILittleLemonReservationRepository, LittleLemonReservationRepository>();
builder.Services.AddScoped<ILittleLemonGuestRepository, LittleLemonGuestRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRockLegendsGenresRepository, RockLegendsGenresRepository>();

builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<IGameGenreRepository, GameGenreRepository>();

builder.Services.AddScoped<IJdmRepository, JdmRepository>();
builder.Services.AddScoped<IJdmManufacturerRepository, JdmManufacturerRepository>();

builder.Services.AddHttpContextAccessor();

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

app.UseAuthentication();    // Who Am I?
app.UseAuthorization();     // May I?

app.UseMiddleware<MyLocalizationMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
