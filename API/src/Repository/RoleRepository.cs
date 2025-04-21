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
    public class RoleRepository : IRoleRepository
    {
        private readonly AplicationDbContext _context;
        public RoleRepository(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> createRole(string roleName)
        {
            var exists = await _context.Roles.AnyAsync(r => r.Name == roleName);
            if (exists || string.IsNullOrWhiteSpace(roleName))
            {
                return false;
            }

            var role = new Role { Name = roleName };
            _context.Roles.Add(role);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }


        public async Task<bool> isEmpty()
        {
            return !await _context.Roles.AnyAsync();
        }
    }

}