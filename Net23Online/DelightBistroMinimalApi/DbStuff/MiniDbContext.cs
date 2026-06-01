using Microsoft.EntityFrameworkCore;

namespace DelightBistroMinimalApi.DbStuff
{
    public class MiniDbContext : DbContext
    {
        public DbSet<Tea> Teas { get; set; }
        public MiniDbContext(DbContextOptions<MiniDbContext> options) : base(options) { }
    }
}
