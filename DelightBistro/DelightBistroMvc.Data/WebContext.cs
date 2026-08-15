using Microsoft.EntityFrameworkCore;
using DelightBistroMvc.Data.DataModels;
using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Data
{
    public class WebContext : DbContext
    {
        public DbSet<UserData> Users { get; set; }
        public DbSet<FoodItemData> FoodItems { get; set; }
        public DbSet<IngredientData> Ingredients { get; set; }
        public DbSet<MenuData> Menus { get; set; }
        public DbSet<OrderData> Orders { get; set; }
        public DbSet<NotificationData> Notifications { get; set; }

        public WebContext(DbContextOptions<WebContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationData>()
             .HasOne(x => x.Author)
             .WithMany(x => x.Notifications);

            modelBuilder.Entity<NotificationData>()
                .HasIndex(n => new { n.IsActive, n.TimeToPublish });

            modelBuilder.Entity<UserData>()
              .HasOne(x => x.UserProfile)
              .WithOne(x => x.User)
              .HasForeignKey<UserData>(x => x.UserProfileId);

            modelBuilder.Entity<UserData>()
                .HasMany(x => x.MyFriends)
                .WithMany(x => x.WhoIsMyFriends);

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
                .Property(f => f.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<FoodItemData>()
               .HasOne(x => x.Creator)
               .WithMany(x => x.CreatedFoodItems)
               .HasForeignKey(x => x.CreatorId);

            modelBuilder.Entity<IngredientData>()
                .Property(f => f.Price)
                .HasPrecision(18, 2);

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
                    j.Property(y => y.QuantityOfIngredients)
                    .HasPrecision(18, 2)
                    .HasDefaultValue(10);

                    j.HasKey(t => new { t.FoodItemDataId, t.IngredientDataId });
                    j.ToTable("FoodItemIngredientDatas");
                });

            modelBuilder.Entity<OrderData>()
                .Property(od => od.TotalPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderData>()
                .HasIndex(o => o.CreatedDateTime);

            modelBuilder.Entity<OrderData>()
                .HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OrderData>()
                .HasMany(x => x.FoodItems)
                .WithMany(x => x.Orders);

            base.OnModelCreating(modelBuilder);
        }
    }
}