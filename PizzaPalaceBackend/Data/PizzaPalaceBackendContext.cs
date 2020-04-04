using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PizzaPalaceBackend.Model;

namespace PizzaPalaceBackend.Data
{
    public class PizzaPalaceBackendContext : DbContext
    {
        public PizzaPalaceBackendContext (DbContextOptions<PizzaPalaceBackendContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Model.OrderItem>().HasKey(sc => new { sc.OrderID, sc.ItemID });
        }

        public DbSet<PizzaPalaceBackend.Model.Category> Category { get; set; }

        public DbSet<PizzaPalaceBackend.Model.Item> Item { get; set; }

        public DbSet<PizzaPalaceBackend.Model.Order> Order { get; set; }

        public DbSet<PizzaPalaceBackend.Model.OrderItem> OrderItem { get; set; }
    }
}
