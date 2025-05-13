using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.src.Data;
using API.src.DTOs;
using API.src.Helpers;
using API.src.Interface;
using API.src.Mappers;
using API.src.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.src.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        public UserRepository(AplicationDbContext context, UserManager<User> userManager, ITokenService tokenService)
        {
            _context = context;
            _userManager = userManager;
            _tokenService = tokenService;
        }
        public async Task<bool> isEmpty()
        {
            return !await _context.Users.AnyAsync();
        }

        public async Task<bool> CreateAdmin(CreateUserDTO userDTO)
        {
            var user = UserMapper.CreateUserToUser(userDTO);

            await _userManager.CreateAsync(user, userDTO.Password);

            var roleAdminResult = await _userManager.AddToRoleAsync(user, "Admin");
            var roleUserResult = await _userManager.AddToRoleAsync(user, "User");

            return roleAdminResult.Succeeded && roleUserResult.Succeeded;
        }

        public async Task<AuthenticatedUserDto> RegisterUserAsync(RegisterDTO newUser)
        {

            var user = UserMapper.RegisterToUser(newUser);

            if (string.IsNullOrEmpty(newUser.Password) || string.IsNullOrEmpty(newUser.ConfirmPassword))
            {
                throw new ArgumentException("La contraseña y la confirmación son requeridas");
            }

            var createUser = await _userManager.CreateAsync(user, newUser.Password);

            if (!createUser.Succeeded)
            {
                throw new InvalidOperationException("Error al crear el usuario: " + string.Join(", ", createUser.Errors.Select(e => e.Description)));
            }

            var roleUser = await _userManager.AddToRoleAsync(user, "User");
            if (!roleUser.Succeeded)
            {
                throw new InvalidOperationException("Error al asignar el rol: " + string.Join(", ", roleUser.Errors.Select(e => e.Description)));

            }

            var role = await _userManager.GetRolesAsync(user);
            var roleName = role.FirstOrDefault() ?? "User";

            var token = _tokenService.GenerateToken(user, roleName);
            return UserMapper.UserToAuthenticatedDto(user, token);
        }

        public async Task<AuthenticatedUserDto> LoginUserAsync(LoginDTO loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new ArgumentException("Correo o contraseña inválidos");
            }

            if (!user.IsActive)
            {
                throw new ArgumentException("Tu cuenta está deshabilitada. Contacta al administrador.");
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
            {
                throw new ArgumentException("Correo o contraseña inválidos");
            }


            user.lastLogin = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var roleName = roles.FirstOrDefault() ?? "User";

            var token = _tokenService.GenerateToken(user, roleName);
            return UserMapper.UserToAuthenticatedDto(user, token);

        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado");
            }
            return user;
        }

    }

}