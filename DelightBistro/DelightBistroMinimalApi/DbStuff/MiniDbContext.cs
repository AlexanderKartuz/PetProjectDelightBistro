using DelightBistroMinimalApi.DbStuff.Models;
using Microsoft.EntityFrameworkCore;

namespace DelightBistroMinimalApi.DbStuff
{
    public class MiniDbContext : DbContext
    {
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<SeriLogEntry>? LogEntries { get; set; }
        public MiniDbContext(DbContextOptions<MiniDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SeriLogEntry>(entity =>
            {
                entity.Property(e => e.Properties).HasColumnType("Xml");
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("GetDate()");
            });

            modelBuilder.Entity<Drink>(entity =>
            {
                entity.Property(e => e.Price).HasPrecision(18, 2);
            });

            base.OnModelCreating(modelBuilder);
        }
    }

}
