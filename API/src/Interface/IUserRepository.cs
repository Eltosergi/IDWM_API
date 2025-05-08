using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.src.DTOs;
using API.src.Models;

using Microsoft.AspNetCore.Mvc;

namespace API.src.Interface
{
    public interface IUserRepository
    {
        Task<bool> isEmpty();
        Task<bool> CreateAdmin(CreateUserDTO userDTO);

        Task<AuthenticatedUserDto> RegisterUserAsync(RegisterDTO newUser);
        Task<AuthenticatedUserDto> LoginUserAsync(LoginDTO loginDto);
        Task<User> GetUserByIdAsync(int id);
    }
}