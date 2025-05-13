using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.src.DTOs;
using API.src.Helpers;
using API.src.Interface;
using API.src.Mappers;
using API.src.Models;

namespace API.src.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;

        public CartRepository(IUserRepository userRepository, IProductRepository productRepository)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
        }
        public async Task<InsertionProdutCartDTO> AddProductToCart(int userId, int productId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId) ?? throw new ArgumentException("User not found");
            var product = await _productRepository.GetProductByIdAsync(productId) ?? throw new ArgumentException("Product not found");
            
            var item = new CartProduct
            {
                ProductId = productId
            };
            user.Cart.Products.Add(item);

            return await Task.FromResult(ProductMapper.UserProductToInsertionProductCartDTO(user, product));
        }

        public async Task<ICollection<CartProductDTO>> GetCartByUserId(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId) ?? throw new ArgumentException("User not found");
            return UserMapper.CartToListCartProductDTO(user.Cart);
        }

    }
}