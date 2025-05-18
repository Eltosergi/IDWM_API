using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using API.src.Data;
using API.src.DTOs;
using API.src.Helpers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.src.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(UnitofWork unitofWork) : ControllerBase
    {
        private readonly UnitofWork _unitofWork = unitofWork;

        [HttpGet("Cart")]
        [Authorize]
        public async Task<IActionResult> getCart()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                                ?? throw new ArgumentNullException("User ID not found"));
                var cart = await _unitofWork.CartRepository.GetCartByUserId(userId);
                if (cart.Count == 0)
                {
                    return Ok(new ApiResponse<string>(false, "No se encontraron productos en el carrito", "Vacío"));
                }

                return Ok(new ApiResponse<ICollection<CartProductDTO>>(true, "Carrito encontrado", cart));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }

        }

        [HttpPost("AddProduct")]
        [Authorize]
        public async Task<IActionResult> AddProductToCart([FromBody] AddProductToCartDTO dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                                ?? throw new ArgumentNullException("User ID not found"));

                var cart = await _unitofWork.CartRepository.AddProductToCart(userId, dto.ProductId);

                if (cart == null)
                {
                    return NotFound(new { message = "No se encontró el carrito o el producto no pudo ser agregado." });
                }

                return Ok(Ok(new ApiResponse<InsertionProdutCartDTO>(true, "Producto agregado al carrito", cart)));
            }

            catch (ArgumentNullException ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }

        }

    }
}