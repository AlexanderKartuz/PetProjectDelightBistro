using Microsoft.EntityFrameworkCore;

namespace MovieMinimalApi.DbStuff
{
    public class MiniDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public MiniDbContext(DbContextOptions<MiniDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>()
                .HasIndex(t => t.Name)
                .IsUnique();

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Tags)
                .WithMany(t => t.Movies)
                .UsingEntity(j => j.ToTable("MovieTag"));
        }
    }
}
