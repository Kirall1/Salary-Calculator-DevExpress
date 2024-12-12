using Microsoft.EntityFrameworkCore;
using SqliteEncryptionApp.Models;

namespace SqliteEncryptionApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=example.db;Password=Password12!");
        }
    }
}
