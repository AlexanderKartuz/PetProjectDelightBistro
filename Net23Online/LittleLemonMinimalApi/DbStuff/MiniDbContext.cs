using Microsoft.EntityFrameworkCore;

namespace LittleLemonMinimalApi.DbStuff
{
    public class MiniDbContext : DbContext
    {
        public DbSet<MenuItem> MenuItems { get; set; }

        public MiniDbContext(DbContextOptions<MiniDbContext> options) : base(options) { }
    }
}
