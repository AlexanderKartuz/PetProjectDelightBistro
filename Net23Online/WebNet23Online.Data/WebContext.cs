using Microsoft.EntityFrameworkCore;
using WebNet23Online.Data.DataModels;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Models.AnimalWorld;
using WebNet23Online.Data.Models.Steam;

namespace WebNet23Online.Data
{
    public class WebContext : DbContext
    {
        public DbSet<AnimeGirlData> AnimeGirls { get; set; }
        public DbSet<AnimeData> Animes { get; set; }
        public DbSet<AnimeStudioData> AnimeStudios { get; set; }
        public DbSet<UserData> Users { get; set; }
        public DbSet<MazeData> Mazes { get; set; }
        public DbSet<HabitTrackerProfileData> HabitTrackerProfile { get; set; }
        public DbSet<HabitData> Habits { get; set; }
        public DbSet<HabitDoneDatesData> HabitDoneDates { get; set; }
        public DbSet<HabitTrackerDiaryData> DiaryEntries { get; set; }
        public DbSet<AnimalFamilyData> AnimalFamilies { get; set; }
        public DbSet<AnimalSpeciesData> AnimalSpecies { get; set; }
        public DbSet<ZooData> Zoos { get; set; }
        public DbSet<LittleLemonData> LittleLemon { get; set; }
        public DbSet<LittleLemonGuestData> LittleLemonGuests { get; set; }
        public DbSet<RockBandsData> RockBand { get; set; }
        public DbSet<FoodItemData> FoodItems { get; set; }
        public DbSet<IngredientData> Ingredients { get; set; }
        public DbSet<MenuData> Menus { get; set; }
        public DbSet<GenreOfRockBandsData> RockBandGenresDictionary { get; set; }
        public DbSet<RockBandGenreData> RockBandGenres { get; set; }

        public DbSet<RockLegendsData> RockLegends { get; set; }
        public DbSet<RockLegendsGenres> RockLegendsGenres { get; set; }

        public DbSet<SlayTheSpire2HeroesData> SlayTheSpire2Heroes { get; set; }

        public DbSet<GameData> Games { get; set; }
        public DbSet<PublisherData> Publishers { get; set; }
        public DbSet<GameGenreData> GameGenres { get; set; }
        public DbSet<GameReviewData> GameReviews { get; set; }

        public DbSet<JdmCarsData> JdmCars { get; set; }
        public DbSet<JdmManufacturerData> JdmManufacturer { get; set; }
        public DbSet<JdmCarsBlogCommentsData> JdmCarsBlogComments { get; set; }

        public WebContext(DbContextOptions<WebContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnimeData>()
                .HasMany(x => x.Heroes)
                .WithMany(x => x.Animes);

            modelBuilder.Entity<AnimeStudioData>()
                .HasMany(x => x.Animes)
                .WithOne(x => x.Studio)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AnimalFamilyData>()
                .HasMany(x => x.Species)
                .WithOne(x => x.AnimalFamily)
                .HasForeignKey(x => x.AnimalFamilyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AnimalFamilyData>()
                .HasOne(x => x.Creator)
                .WithMany(x => x.CreatedByMeAnimalFamilies)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ZooData>()
                .HasMany(x => x.AnimalSpecies)
                .WithMany(x => x.ZooData)
                .UsingEntity(x => x.ToTable("BindZooAndAnimalSpecies"));

            modelBuilder.Entity<ZooData>()
                .HasOne(x => x.Creator)
                .WithMany(x => x.CreatedByMeZoos)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AnimalSpeciesData>()
                .HasOne(x => x.Creator)
                .WithMany(x => x.CreatedByMeAnimalSpecies)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserData>()
                .HasOne(x => x.UserProfile)
                .WithOne(x => x.User)
                .HasForeignKey<UserData>(x => x.UserProfileId);

            modelBuilder.Entity<UserData>()
                .HasMany(x => x.MyFriends)
                .WithMany(x => x.WhoIsMyFriends);

            modelBuilder.Entity<HabitData>()
                .HasMany(x => x.CompletedDates)
                .WithOne(x => x.Habit);

            modelBuilder.Entity<UserData>()
                .HasMany(x => x.DiaryEntries)
                .WithOne(x => x.User);

            modelBuilder.Entity<UserData>()
                .HasOne(x => x.HabitTrackerProfile)
                .WithOne(x => x.User);

            modelBuilder.Entity<UserData>()
                .HasMany(x => x.Habits)
                .WithOne(x => x.User);

            //Delight Bistro
            modelBuilder.Entity<MenuData>()
                .HasMany(x => x.FoodItems)
                .WithOne(x => x.MenuData);

            // used Links
            //modelBuilder.Entity<FoodItemData>()
            //    .HasMany(x => x.IngredientsList)
            //    .WithMany(x => x.FoodItems);

            modelBuilder.Entity<MenuData>()
                .HasOne(x => x.Creator)
                .WithMany(x => x.CreatedMenus)
                .HasForeignKey(x => x.CreatorId);

            modelBuilder.Entity<FoodItemData>()
               .HasOne(x => x.Creator)
               .WithMany(x => x.CreatedFoodItems)
               .HasForeignKey(x => x.CreatorId);

            modelBuilder.Entity<IngredientData>()
               .HasOne(x => x.Creator)
               .WithMany(x => x.CreatedIngredients)
               .HasForeignKey(x => x.CreatorId);

            // new Links
            modelBuilder.Entity<FoodItemData>()
            .HasMany(fi => fi.IngredientsList)
            .WithMany(i => i.FoodItems)
            .UsingEntity<FoodItemIngredientData>(
                j => j.HasOne(y => y.IngredientData)
                    .WithMany(z => z.FoodItemIngredientDatas)
                    .HasForeignKey(y => y.IngredientDataId),
                j => j.HasOne(y => y.FoodItemData)
                    .WithMany(t => t.FoodItemIngredientDatas)
                    .HasForeignKey(y => y.FoodItemDataId),
                j =>
                {
                    j.Property(y => y.QuantityOfIngredients).HasDefaultValue(10);
                    j.HasKey(t => new { t.FoodItemDataId, t.IngredientDataId });
                    j.ToTable("FoodItemIngredientDatas");
                });

            modelBuilder.Entity<RockLegendsData>()
                .HasOne(x => x.Genres)
                .WithMany(x => x.Groups)
                .HasForeignKey(x => x.RockLegendsGenresId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<GameData>()
               .HasOne(x => x.Publisher)
               .WithMany(x => x.Games)
               .HasForeignKey(x => x.PublisherId);

            modelBuilder.Entity<GameData>()
               .HasOne(x => x.CreatedByUser)
               .WithMany(x => x.CreatedGames)
               .HasForeignKey(x => x.CreatedByUserId);

            modelBuilder.Entity<GameData>()
               .HasOne(x => x.ModifiedByUser)
               .WithMany(x => x.ModifiedGames)
               .HasForeignKey(x => x.ModifiedByUserId);

            modelBuilder.Entity<GameReviewData>()
                .HasOne(x => x.Author)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GameReviewData>()
                .HasOne(x => x.Game)
                .WithMany(x => x.GameReviews)
                .HasForeignKey(x => x.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LittleLemonData>()
                .HasOne(x => x.Guest)
                .WithMany(x => x.GuestLittleLemonReservations)
                .HasForeignKey(x => x.GuestId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<LittleLemonData>()
                .HasOne(x => x.CreatedByUser)
                .WithMany(x => x.UserAccountLittleLemonReservations)
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RockBandGenreData>()
                .HasKey(x => new { x.RockBandId, x.GenreId });

            modelBuilder.Entity<RockBandGenreData>()
                .HasOne(x => x.RockBand)
                .WithMany(x => x.RockBandGenres)
                .HasForeignKey(x => x.RockBandId);

            modelBuilder.Entity<RockBandGenreData>()
                .HasOne(x => x.Genre)
                .WithMany(x => x.RockBandGenres)
                .HasForeignKey(x => x.GenreId);

            modelBuilder.Entity<GenreOfRockBandsData>()
                .HasIndex(x => x.Name)
                .IsUnique();

            modelBuilder.Entity<JdmCarsData>()
                .HasOne(x => x.JdmManufacturerData)
                .WithMany(x => x.JdmCarsDatas)
                .HasForeignKey(x => x.JdmManufacturerDataId);

            modelBuilder.Entity<JdmCarsData>()
                 .HasOne(x => x.Creator)
                 .WithMany(x => x.CreatedByCarsJdm)
                 .HasForeignKey(x => x.CreatorId);

            modelBuilder.Entity<JdmCarsBlogCommentsData>()
                .HasOne(x => x.User)
                .WithMany(u => u.JournalComments)
                .HasForeignKey(x => x.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
