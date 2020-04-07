using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PizzaPalace.Model;

namespace PizzaPalace.Data
{
    public class PizzaPalaceContext : DbContext
    {
        public PizzaPalaceContext (DbContextOptions<PizzaPalaceContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Model.OrderItem>().HasKey(sc => new { sc.OrderID, sc.ItemID });
        }

        public DbSet<PizzaPalace.Model.Category> Category { get; set; }

        public DbSet<PizzaPalace.Model.Item> Item { get; set; }

        public DbSet<PizzaPalace.Model.Order> Order { get; set; }

        public DbSet<PizzaPalace.Model.OrderItem> OrderItem { get; set; }
    }
}
