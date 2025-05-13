using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using API.src.Data;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(UnitofWork unitofWork) : ControllerBase
    {
        private readonly UnitofWork _unitofWork = unitofWork;

        [HttpGet("Cart")]
        [Authorize]
        public IActionResult getCart()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                                ?? throw new ArgumentNullException("User ID not found"));
            var cart = _unitofWork.CartRepository.GetCartByUserId(userId);

            if (cart == null)
            {
                return NotFound(new { message = "No se encontró el carrito" });
            }

            return Ok(cart);
                        
        }
    }
}