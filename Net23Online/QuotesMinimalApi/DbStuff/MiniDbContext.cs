using Microsoft.EntityFrameworkCore;

namespace QuotesMinimalApi.DbStuff
{
    public class MiniDbContext : DbContext
    {
        public DbSet<Quote> Quotes { get; set; }
        public MiniDbContext(DbContextOptions<MiniDbContext> options) : base(options) { }
    }
}