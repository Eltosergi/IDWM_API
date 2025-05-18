using API.src.Data;
using API.src.DTOs;
using API.src.Helpers;
using API.src.Interface;
using API.src.Mappers;
using API.src.Models;

using Microsoft.EntityFrameworkCore;

namespace API.src.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly AplicationDbContext _context;

        public CartRepository(IUserRepository userRepository, IProductRepository productRepository, AplicationDbContext context)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _context = context;
        }
        public async Task<InsertionProdutCartDTO> AddProductToCart(int userId, int productId)
        {
            var user = await _context.Users
                .Include(u => u.Cart)
                    .ThenInclude(c => c.Products)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var product = await _productRepository.GetProductByIdAsync(productId)
                ?? throw new ArgumentException("Product not found");

            var cart = user?.Cart ?? throw new ArgumentException("Cart not found");

            var existingCartProduct = cart.Products.FirstOrDefault(cp => cp.ProductId == productId);

            if (existingCartProduct != null)
            {
                existingCartProduct.Quantity += 1;
            }
            else
            {
                var newItem = new CartProduct
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = 1
                };
                cart.Products.Add(newItem);
            }

            _context.Update(cart);
            await _context.SaveChangesAsync();

            return ProductMapper.UserProductToInsertionProductCartDTO(user, product);
        }

        public async Task<ICollection<CartProductDTO>> GetCartByUserId(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Cart)
                    .ThenInclude(c => c.Products)
                        .ThenInclude(cp => cp.Product)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var cart = user?.Cart ?? throw new ArgumentException("Cart not found");

            return UserMapper.CartToCartProductDTOs(cart);
        }

        public async Task<bool> RemoveProductFromCart(int userId, int productId)
        {
            var user = await _context.Users
                .Include(u => u.Cart)
                    .ThenInclude(c => c.Products)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return false;

            var cart = user?.Cart ?? throw new ArgumentException("Cart not found");

            var productToRemove = cart.Products.FirstOrDefault(cp => cp.ProductId == productId);

            if (productToRemove == null)
                return false;

            if (productToRemove.Quantity > 1)
            {
                productToRemove.Quantity -= 1;
                _context.Update(cart);
            }
            else
            {
                cart.Products.Remove(productToRemove);
                _context.Remove(productToRemove);
            }



            await _context.SaveChangesAsync();

            return true;
        }

    }
}