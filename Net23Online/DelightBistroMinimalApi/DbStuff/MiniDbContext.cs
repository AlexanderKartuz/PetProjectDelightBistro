using Microsoft.EntityFrameworkCore;

namespace DelightBistroMinimalApi.DbStuff
{
    public class MiniDbContext : DbContext
    {
        public DbSet<Tea> Teas { get; set; }
        public DbSet<SeriLogEntry>? LogEntries { get; set; }
        public MiniDbContext(DbContextOptions<MiniDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SeriLogEntry>(entity =>
            {
                entity.Property(e => e.Properties).HasColumnType("Xml");
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("GetDate()");
            });

            base.OnModelCreating(modelBuilder);
        }
    }

}
