using Microsoft.EntityFrameworkCore;

namespace AnimalWorldMinimalApi.DbStuff
{
    public class AnimalWorldDbContext : DbContext
    {
        public DbSet<InterestingFact> InterestingFacts { get; set; }

        public AnimalWorldDbContext(DbContextOptions<AnimalWorldDbContext> options) : base(options) { }
    }
}
