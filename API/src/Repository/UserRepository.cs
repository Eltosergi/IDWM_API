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
    public class UserRepository : IUserRepository
    {
        private readonly AplicationDbContext _context;
        public UserRepository(AplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> isEmpty()
        {
            return !await _context.Users.AnyAsync();
        }

        public async Task<bool> createUser(CreateUserDTO userDTO)
        {
            var user = new User
            {
                Name = userDTO.Name,
                LastName = userDTO.LastName,
                BirthDate = userDTO.BirthDate,
                Email = userDTO.Email,
                Password = userDTO.Password,
                RoleId = userDTO.RoleId
            };
            await _context.Users.AddAsync(user);
            return await _context.SaveChangesAsync() > 0;
        }
    }

}