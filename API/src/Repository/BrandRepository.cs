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
    public class BrandRepository : IBrandRepository
    {
        private readonly AplicationDbContext _context;

        public BrandRepository(AplicationDbContext context)
        {
            _context = context;
        }

        public Task<bool> AddBrand(string brand)
        {
            var newBrand = new Brand { Name = brand };
            _context.Brands.Add(newBrand);
            return _context.SaveChangesAsync().ContinueWith(t => t.Result > 0);
        }

        public async Task<bool> isEmpty()
        {
            return !await _context.Brands.AnyAsync();
        }
    }

}