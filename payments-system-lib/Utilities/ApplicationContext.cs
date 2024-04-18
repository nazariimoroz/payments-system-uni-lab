﻿using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<BaseCard> ClientCards => Set<BaseCard>();
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
        }
    }
}
