using Microsoft.EntityFrameworkCore;

namespace MovieMinimalApi.DbStuff
{
    public class MiniDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public MiniDbContext(DbContextOptions<MiniDbContext> options) : base(options) { }
    }
}
