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

        public async Task<bool> SeederUser(SeederUserDTO userDTO, string role)
        {
            var user = UserMapper.CreateUserToUser(userDTO);
            await _userManager.CreateAsync(user, userDTO.Password);

            if (role == "Admin" || role == "User")
            {
                await _userManager.AddToRoleAsync(user, role);
                return true;
            }
            await _context.SaveChangesAsync();
            return false;
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

            await _context.SaveChangesAsync();

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
            var user = await _context.Users.FindAsync(id) ?? throw new ArgumentException("User not found");

            return user;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordDTO changePasswordDto, int userId)
        {
            if (changePasswordDto == null ||
            string.IsNullOrWhiteSpace(changePasswordDto.CurrentPassword) ||
            string.IsNullOrWhiteSpace(changePasswordDto.NewPassword))
            {
                throw new ArgumentException("Datos de cambio de contraseña inválidos.");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new ArgumentException("Usuario no encontrado");
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

            return result.Succeeded;
        }

        public IQueryable<User> GetQueryableProducts()
        {
            return _context.Users.AsQueryable().Include(u => u.Addresses);
        }

        public async Task<UserDTO> GetById(int id)
        {
            var user = await _context.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new ArgumentException("Usuario no encontrado");
            }

            return UserMapper.UserToUserDTO(user);
        }

        public async Task<UserDTO> UpdateUser(UserDTO user)
        {
            var userToUpdate = await _context.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (userToUpdate == null)
            {
                throw new ArgumentException("Usuario no encontrado");
            }

            userToUpdate.Name = user.FirtsName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.Email = user.Email;
            userToUpdate.PhoneNumber = user.Thelephone;
            userToUpdate.BirthDate = user.BirthDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
            userToUpdate.RegisteredAt = DateTime.UtcNow;
            ;
            await _context.SaveChangesAsync();

            return UserMapper.UserToUserDTO(userToUpdate);

        }

        public async Task<UserDTO> DeleteUser(int Id, string reason)
        {
            var userToDelete = await _context.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == Id);

            if (userToDelete == null)
            {
                throw new ArgumentException("Usuario no encontrado");
            }

            userToDelete.IsActive = false;
            userToDelete.ResonDeactivation = reason;

            await _context.SaveChangesAsync();

            return UserMapper.UserToUserDTO(userToDelete);

        }
    }

}