using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.src.DTOs;

namespace API.src.Interface
{
    public interface ICartRepository
    {
        public Task<ICollection<CartProductDTO>> GetCartByUserId(int userId);
        public Task<InsertionProdutCartDTO> AddProductToCart(int userId, int productId);
    }
}