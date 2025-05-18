using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.src.Data;
using API.src.Interface;
using API.src.Models;

using Microsoft.EntityFrameworkCore;

namespace API.src.Repository
{
    public class CategoryRepository(AplicationDbContext aplicationDbContext) : ICategoryRepository
    {
        private readonly AplicationDbContext _context = aplicationDbContext;


        public async Task<bool> isEmpty()
        {
            return !await _context.Categories.AnyAsync();
        }

        public Task<bool> SeedCategory(string name)
        {
            var Category = new Category
            {
                Name = name
            };
            _context.Categories.Add(Category);
            return _context.SaveChangesAsync().ContinueWith(t => t.Result > 0);
        }
    }
}