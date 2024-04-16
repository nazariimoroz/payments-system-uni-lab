using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using payments_system_ui.Objects;
using payments_system_ui.Users;

namespace payments_system_ui.Utilities
{
    public class DbException : Exception
    {
        public DbException(string reason) : base(reason) { }
    }

    public class ApplicationContext : DbContext
    {
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<CreditCard> CreditCards => Set<CreditCard>();
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .SetBasePath(Directory.GetCurrentDirectory())
                .Build();

            optionsBuilder.UseMySql(config.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
