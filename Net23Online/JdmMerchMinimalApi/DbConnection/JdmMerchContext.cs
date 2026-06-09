using JdmMerchMinimalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JdmMerchMinimalApi.DbConnection
{
    public class JdmMerchContext : DbContext
    {
        public DbSet<JdmMerchModel> jdmMerches { get; set; }
        public JdmMerchContext(DbContextOptions<JdmMerchContext> options) : base(options) { }
    }
}