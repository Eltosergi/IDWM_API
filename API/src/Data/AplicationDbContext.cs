using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.src.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.src.Data
{
    public class AplicationDbContext(DbContextOptions<AplicationDbContext> options) : IdentityDbContext<User, Role, int>(options)
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Image> Images { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CartProduct>().HasKey(cp => new { cp.ProductId, cp.CartId });

            List<Role> roles = new List<Role>()
            {
                new Role() { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                new Role() { Id = 2, Name = "User", NormalizedName = "USER" },
            };

            modelBuilder.Entity<Role>().HasData(roles);
        }

    }
}