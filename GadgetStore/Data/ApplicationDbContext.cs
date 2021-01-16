using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadgetStore.Models;

namespace GadgetStore.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<CPU> CPUs { get; set; }
        public DbSet<Diagonal> Diagonals { get; set; }
        public DbSet<Gadget> Gadgets { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<GadgetType> gadgetTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<ScreenResolution> ScreenResolutions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OrderStatus>().HasData(
                new OrderStatus[]
                {
                    new OrderStatus { Id = 1, Name = "ожидает отправки"},
                    new OrderStatus { Id = 2, Name = "отправлен"},
                    new OrderStatus { Id = 3, Name = "прибыл"}
                });
            modelBuilder.Entity<GadgetType>().HasData(
               new GadgetType[]
               {
                    new GadgetType { Id = 1, Name = "Смартфон"},
                    new GadgetType { Id = 2, Name = "Планшет"}
               });

        }
    }
}
