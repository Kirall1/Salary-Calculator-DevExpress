using Microsoft.EntityFrameworkCore;
using SalaryCalculator.Models;

namespace SalaryCalculator.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() { }

        public DbSet<SalaryDetail> SalaryDetails { get; set; }

        public DbSet<RankCoefficient> RankCoefficients { get; set; }

        public DbSet<AdditionToSalary> AdditionToSalaries {  get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=data.db")
                          .UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
