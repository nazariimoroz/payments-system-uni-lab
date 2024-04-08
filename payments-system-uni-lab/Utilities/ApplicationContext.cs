using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using payments_system_uni_lab.Objects;
using payments_system_uni_lab.Users;

namespace payments_system_uni_lab.Utilities
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

            optionsBuilder.UseMySql(config.GetConnectionString("WithPasswordConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
