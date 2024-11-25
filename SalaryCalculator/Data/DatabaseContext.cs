using Microsoft.EntityFrameworkCore;
using SalaryCalculator.Models;

namespace SalaryCalculator.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<SalaryDetail> SalaryDetails { get; set; }

        public DbSet<RankCoefficient> RankCoefficients { get; set; }
    }
}
