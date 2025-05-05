using API.src.DTOs;
using API.src.Helpers;
using API.src.Interface;
using API.src.Mappers;
using API.src.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace API.src.Controllers
{
    public class AuthController(ILogger<AuthController> logger, IUserRepository userRepository) : BaseController
    {
        private readonly ILogger<AuthController> _logger = logger;

        private readonly IUserRepository _userRepository = userRepository;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO newUser)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponse<string>(false, "Datos inválidos", null, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
                var userDto = await _userRepository.RegisterUserAsync(newUser);
                return Ok(new ApiResponse<AuthenticatedUserDto>(true, "Usuario registrado exitosamente", userDto));
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error de validación en registro de usuario");
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error en lógica de negocio al registrar usuario");
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al registrar usuario");
                return StatusCode(500, new ApiResponse<string>(false, "Ocurrió un error inesperado"));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponse<string>(false, "Datos inválidos", null, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

                var userDto = await _userRepository.LoginUserAsync(loginDto);
                return Ok(new ApiResponse<AuthenticatedUserDto>(true, "Login exitoso", userDto));
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error de validación en login de usuario");
                return Unauthorized(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(false, "Error interno del servidor", null, new List<string> { ex.Message }));
            }
        }

    }
}