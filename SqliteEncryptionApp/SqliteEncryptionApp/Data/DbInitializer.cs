using SqliteEncryptionApp.Models;
using System;
using System.Linq;

namespace SqliteEncryptionApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            try
            {
                context.Database.EnsureCreated();
                if (!context.Users.Any())
                {
                    context.Users.Add(new User { Name = "Name1" });
                    context.Users.Add(new User { Name = "Name2" });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
