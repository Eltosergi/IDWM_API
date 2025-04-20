using System;
using System.Collections.Generic;
using System.Linq;
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
        public Task<bool> CreateProduct(CreateProductDTO product)
        {
            var newProduct = new Product
            {
                Name = product.Name,
                Price = product.Price,
                BrandId = product.BrandId,
                ConditionId = product.ConditionId
            };
            _context.Products.Add(newProduct);
            return _context.SaveChangesAsync().ContinueWith(t => t.Result > 0);
        }

        public async Task<bool> isEmpty()
        {
            return !await _context.Products.AnyAsync();
        }
    }
    
}