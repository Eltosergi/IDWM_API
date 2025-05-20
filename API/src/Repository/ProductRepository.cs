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

        public async Task<SuccessProduct> CreateProduct(CreateProductDTO createProductDTO)
        {
            var SuccessProduct = new SuccessProduct
            {
                Name = createProductDTO.Name,
                Price = createProductDTO.Price,
                Description = createProductDTO.Description,
                Stock = createProductDTO.Stock,
                Condition = createProductDTO.Condition,
                Brand = createProductDTO.Brand
            };
            var condition = _context.Conditions.FirstOrDefault(c => c.Name == createProductDTO.Condition);
            if (condition == null)
            {
                condition = new Condition { Name = createProductDTO.Condition };
                _context.Conditions.Add(condition);
                _context.SaveChanges();
            }

            var brand = _context.Brands.FirstOrDefault(b => b.Name == createProductDTO.Brand);
            if (brand == null)
            {
                brand = new Brand { Name = createProductDTO.Brand };
                _context.Brands.Add(brand);
                _context.SaveChanges();
            }

            var categories = new List<Category>();
            foreach (var categoryName in createProductDTO.Categories)
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
                if (category == null)
                {
                    category = new Category { Name = categoryName };
                    _context.Categories.Add(category);
                    await _context.SaveChangesAsync();
                }
                categories.Add(category);
            }

            var product = new Product
            {
                Name = createProductDTO.Name,
                Price = createProductDTO.Price,
                Description = createProductDTO.Description,
                Stock = createProductDTO.Stock,
                IsActive = createProductDTO.IsActive,
                ConditionId = condition.Id,
                BrandId = brand.Id,
                Images = createProductDTO.ImageUrl.Select(url => new Image { Url = url }).ToList()
            };

            categories.ForEach(c => product.Categories.Add(c));

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return SuccessProduct;
        }
    }

}