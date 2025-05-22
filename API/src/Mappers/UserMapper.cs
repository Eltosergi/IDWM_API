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
                Cart = new Cart()
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
        public static User CreateUserToUser(SeederUserDTO dto) =>
            new()
            {
                UserName = dto.Email,
                Name = dto.Name,
                LastName = dto.LastName,
                Email = dto.Email,
                BirthDate = dto.BirthDate,
                Cart = new Cart()
            };
        public static GetExampleUserDTO UserToGetExampleUserDto(User user) =>
            new()
            {
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                IsActive = user.IsActive
            };
        public static ICollection<CartProductDTO> CartToCartProductDTOs(Cart cart)
        {
            if (cart == null || cart.Products == null)
                return new List<CartProductDTO>();

            return cart.Products.Select(cp => new CartProductDTO
            {
                Quantity = cp.Quantity,
                Product = new ProductDTO
                {
                    Id = cp.Product?.Id ?? 0,
                    Name = cp.Product?.Name ?? string.Empty,
                    Description = cp.Product?.Description,
                    Price = cp.Product?.Price ?? 0,

                }
            }).ToList();
        }

        public static UserDTO UserToUserDTO(User user)
        {
            return new UserDTO
            {
                FirtsName = user.Name,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                Thelephone = user.PhoneNumber ?? string.Empty,
                BirthDate = user.BirthDate,
                RegisteredAt = user.RegisteredAt,
                LastAccess = user.lastLogin,
                IsActive = user.IsActive
            };
        }

    }
}