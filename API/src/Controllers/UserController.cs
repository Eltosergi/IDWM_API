using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using API.src.Data;
using API.src.DTOs;
using API.src.Extensions;
using API.src.Helpers;
using API.src.Mappers;
using API.src.RequestHelpers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpDelete("RemoveProduct")]
        [Authorize]
        public async Task<IActionResult> RemoveProductFromCart([FromBody] RemoveProduct productdto)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                                ?? throw new ArgumentNullException("User ID not found"));

                var result = await _unitofWork.CartRepository.RemoveProductFromCart(userId, productdto.ProductId);

                if (!result)
                {
                    return NotFound(new ApiResponse<string>(false, "Producto no encontrado en el carrito"));
                }

                return Ok(new ApiResponse<string>(true, "Producto eliminado del carrito"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
        }


        [HttpPost("AddAddress")]
        [Authorize]
        public async Task<IActionResult> AddAddress([FromBody] AddressDTO addressDTO)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                                ?? throw new ArgumentNullException("User ID not found"));


                var result = await _unitofWork.AddressRepository.CreateAddress(addressDTO, userId);

                if (!result)
                {
                    return BadRequest(new ApiResponse<string>(false, "Error al agregar la dirección"));
                }

                return Ok(new ApiResponse<string>(true, "Dirección agregada correctamente"));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
        }

        [HttpGet("GetAddress")]
        [Authorize]
        public async Task<IActionResult> GetAddress()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                                ?? throw new ArgumentNullException("User ID not found"));

                var addresses = await _unitofWork.AddressRepository.GetAddress(userId);

                if (addresses.Count == 0)
                {
                    return Ok(new ApiResponse<string>(false, "No se encontraron direcciones", "Vacío"));
                }

                return Ok(new ApiResponse<ICollection<AddressDTO>>(true, "Direcciones encontradas", addresses));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
        }

        [HttpDelete("DeleteAddress")]
        [Authorize]
        public async Task<IActionResult> DeleteAddress([FromBody] RemoveAddressDTO removeAddressDTO)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                                ?? throw new ArgumentNullException("User ID not found"));

                var result = await _unitofWork.AddressRepository.DeleteAddress(removeAddressDTO.AddressId, userId);

                if (!result)
                {
                    return BadRequest(new ApiResponse<string>(false, "Error al eliminar la dirección"));
                }

                return Ok(new ApiResponse<string>(true, "Dirección eliminada correctamente"));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
        }

        [HttpPost("Purchase")]
        [Authorize]
        public async Task<IActionResult> Purchase([FromBody] SelectionAddressDTO dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                                ?? throw new ArgumentNullException("User ID not found"));

                var result = await _unitofWork.OrderRepository.AddOrder(dto.Selection, userId);

                if (!result)
                {
                    return BadRequest(new ApiResponse<string>(false, "Error al realizar la compra"));
                }

                return Ok(new ApiResponse<string>(true, "Compra realizada correctamente"));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse<string>(false, "Ha ocurrido un error inesperado"));
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] QueryParamsUser userParams)
        {
            var query = _unitofWork.UserRepository.GetQueryableProducts()
                .Filter(userParams.IsActive, userParams.RegisteredFrom, userParams.RegisteredTo, userParams.Id)
                .Search(userParams.SearchTerm)
                .Sort(userParams.OrderBy);

            var total = await query.CountAsync();

            var users = await query
                .Skip((userParams.PageNumber - 1) * userParams.PageSize)
                .Take(userParams.PageSize)
                .ToListAsync();

            var dtos = users.Select(UserMapper.UserToUserDTO).ToList();

            Response.AddPaginationHeader(new PaginationMetaData
            {
                CurrentPage = userParams.PageNumber,
                TotalPages = (int)Math.Ceiling(total / (double)userParams.PageSize),
                PageSize = userParams.PageSize,
                TotalCount = total
            });

            return Ok(new ApiResponse<IEnumerable<UserDTO>>(true, "Usuarios obtenidos correctamente", dtos));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]

        public async Task<ActionResult> GetById(int id)
        {
            var user = await _unitofWork.UserRepository.GetById(id);

            if (user == null)
            {
                return NotFound(new ApiResponse<string>(false, "Usuario no encontrado"));
            }

            return Ok(new ApiResponse<UserDTO>(true, "Usuario encontrado", user));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]

        public async Task<ActionResult> UpdateUser(int id, [FromBody] UserDTO userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest(new ApiResponse<string>(false, "ID de usuario no coincide"));
            }

            var user = await _unitofWork.UserRepository.UpdateUser(userDto);
            if (user == null)
            {
                return NotFound(new ApiResponse<string>(false, "Usuario no encontrado"));
            }

            return Ok(new ApiResponse<UserDTO>(true, "Usuario actualizado correctamente", user));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id, [FromBody] string reason)
        {
            var user = await _unitofWork.UserRepository.GetById(id);
            if (user == null)
            {
                return NotFound(new ApiResponse<string>(false, "Usuario no encontrado"));
            }

            await _unitofWork.UserRepository.DeleteUser(id, reason);
            return Ok(new ApiResponse<string>(true, "Usuario eliminado correctamente"));
        }
    }
}