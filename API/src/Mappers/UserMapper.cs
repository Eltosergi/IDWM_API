using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.src.DTOs;
using API.src.Models;

namespace API.src.Mappers
{
    public class UserMapper
    {
        public static User RegisterToUser(RegisterDTO dto) =>
            new()
            {
                UserName = dto.Email,
                Name = dto.Name,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.Phone,
                BirthDate = dto.BirthDate,
            };
        public static AuthenticatedUserDto UserToAuthenticatedDto(User user, string token) =>
            new()
            {
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                Phone = user.PhoneNumber ?? string.Empty,
                Token = token,
                RegisteredAt = user.RegisteredAt,
                LastAccess = user.lastLogin,
                IsActive = user.IsActive
            };

        public static User CreateUserToUser(CreateUserDTO dto) =>
            new()
            {
                UserName = dto.Email,
                Name = dto.Name,
                LastName = dto.LastName,
                Email = dto.Email,
                BirthDate = dto.BirthDate
            };

        public static GetExampleUserDTO UserToGetExampleUserDto(User user) =>
            new()
            {
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                IsActive = user.IsActive
            };
            
    }
}