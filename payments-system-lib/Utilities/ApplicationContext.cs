using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using payments_system_lib.Classes;
using payments_system_lib.Classes.Cards;
using payments_system_lib.Classes.Users;

namespace payments_system_lib.Utilities
{
    public class DbException : Exception
    {
        public DbException(string reason) : base(reason) { }
    }

    public class ApplicationContext : DbContext
    {
        public DbSet<Client> Client => Set<Client>();
        public DbSet<Admin> Admin => Set<Admin>();
        public DbSet<BaseCard> ClientCard => Set<BaseCard>();
        //public DbSet<Transaction> Transaction => Set<Transaction>();

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
            modelBuilder.Entity<Client>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<Admin>()
                .HasIndex(u => u.Key)
                .IsUnique();
        }
    }
}
