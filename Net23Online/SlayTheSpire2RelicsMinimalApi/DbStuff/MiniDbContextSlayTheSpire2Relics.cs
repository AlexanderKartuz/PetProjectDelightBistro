using Microsoft.EntityFrameworkCore;

namespace SlayTheSpire2RelicsMinimalApi.DbStuff
{
    public class MiniDbContextSlayTheSpire2Relics : DbContext
    {
        public DbSet<Relic> Relics { get; set; }

        public MiniDbContextSlayTheSpire2Relics(DbContextOptions<MiniDbContextSlayTheSpire2Relics> options) : base(options) { }
    }
}
