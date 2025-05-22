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

        public async Task<Product> GetProductById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Brand)
                .Include(p => p.Condition)
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new KeyNotFoundException($"No se encontró el producto con ID {id}");
            return product;
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
                Brand = createProductDTO.Brand,
                Images = createProductDTO.ImageUrl,
                Category = createProductDTO.Categories
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


        public IQueryable<Product> GetQueryableProducts()
        {
            return _context.Products.AsQueryable().Include(p => p.Images)
                .Include(p => p.Brand)
                .Include(p => p.Condition)
                .Include(p => p.Categories);
        }

        public Task<SuccessProduct> UpdateProduct(int id, CreateProductDTO product)
        {

            var existingProduct = _context.Products
                .Include(p => p.Images)
                .Include(p => p.Brand)
                .Include(p => p.Condition)
                .Include(p => p.Categories)
                .FirstOrDefault(p => p.Id == id) ?? throw new ArgumentException("Product not found");

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;
            existingProduct.Stock = product.Stock;
            existingProduct.IsActive = product.IsActive;

            var condition = _context.Conditions.FirstOrDefault(c => c.Name == product.Condition);
            if (condition == null)
            {
                condition = new Condition { Name = product.Condition };
                _context.Conditions.Add(condition);
                _context.SaveChanges();
            }
            existingProduct.ConditionId = condition.Id;

            var brand = _context.Brands.FirstOrDefault(b => b.Name == product.Brand);
            if (brand == null)
            {
                brand = new Brand { Name = product.Brand };
                _context.Brands.Add(brand);
                _context.SaveChanges();
            }
            existingProduct.BrandId = brand.Id;

            var categoriesToRemove = existingProduct.Categories.ToList();
            foreach (var category in categoriesToRemove)
            {
                existingProduct.Categories.Remove(category);
            }

            foreach (var categoryName in product.Categories)
            {
                var category = _context.Categories.FirstOrDefault(c => c.Name == categoryName);
                if (category == null)
                {
                    category = new Category { Name = categoryName };
                    _context.Categories.Add(category);
                    _context.SaveChanges();
                }
                existingProduct.Categories.Add(category);
            }

            foreach (var imageUrl in product.ImageUrl)
            {
                var image = new Image { Url = imageUrl };
                existingProduct.Images.Add(image);
            }

            _context.Products.Update(existingProduct);
            _context.SaveChanges();

            return Task.FromResult(new SuccessProduct
            {
                Name = existingProduct.Name,
                Price = existingProduct.Price,
                Description = existingProduct.Description,
                Stock = existingProduct.Stock,
                Condition = condition.Name,
                Brand = brand.Name,
                Images = existingProduct.Images.Select(i => i.Url).ToList(),
                Category = existingProduct.Categories.Select(c => c.Name).ToList()
            });

        }

        public Task<SuccessProduct> DeleteProduct(int id)
        {
            var product = _context.Products
                .Include(p => p.Images)
                .Include(p => p.Brand)
                .Include(p => p.Condition)
                .Include(p => p.Categories)
                .FirstOrDefault(p => p.Id == id) ?? throw new ArgumentException("Product not found");

            _context.Products.Remove(product);
            _context.SaveChanges();

            return Task.FromResult(new SuccessProduct
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Stock = product.Stock,
                Condition = product.Condition?.Name ?? "",
                Brand = product.Brand?.Name ?? "",
                Images = product.Images.Select(i => i.Url).ToList(),
                Category = product.Categories.Select(c => c.Name).ToList()
            });
        }
    }

}