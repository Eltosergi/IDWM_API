using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.src.Models;

using Microsoft.EntityFrameworkCore;

namespace API.src.Data
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<HistoricCart> HistoricCarts { get; set; }
        public DbSet<HistoricCartProduct> HistoricCartProducts { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CartProduct>()
                .HasKey(cp => new { cp.CartId, cp.ProductId });

            modelBuilder.Entity<HistoricCartProduct>()
                .HasKey(hcp => new { hcp.HistoricCartId, hcp.ProductId });

            modelBuilder.Entity<ProductCategory>()
                .HasKey(pc => new { pc.ProductId, pc.CategoryId });


        }

    }
}