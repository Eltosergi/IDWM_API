using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

using API.src.Data;
using API.src.DTOs;
using API.src.Interface;
using API.src.Models;

using Microsoft.EntityFrameworkCore;

namespace API.src.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AplicationDbContext _context;

        public ProductRepository(AplicationDbContext context)
        {
            _context = context;
        }
        public Task<bool> SeedProduct(SeederProductDTO product)
        {
            var random = new Random();
            var randomCategoryId = random.Next(1, 10); // fuera del LINQ

            var newProduct = new Product
            {
                Name = product.Name,
                Price = product.Price,
                BrandId = product.BrandId,
                ConditionId = product.ConditionId,
                Stock = random.Next(1, 100)
            };

            var category = _context.Categories.FirstOrDefault(c => c.Id == randomCategoryId)
                        ?? throw new ArgumentException("Category not found");

            newProduct.Categories.Add(category);

            _context.Products.Add(newProduct);
            return _context.SaveChangesAsync().ContinueWith(t => t.Result > 0);
        }


        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id) ?? throw new ArgumentException("Product not found"); ;
            return product;
        }

        public async Task<bool> isEmpty()
        {
            return !await _context.Products.AnyAsync();
        }
    }

}