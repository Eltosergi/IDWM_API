using System.Security.Claims;

using API.src.Data;
using API.src.DTOs;
using API.src.Helpers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.src.Controllers
{
    public class AuthController(ILogger<AuthController> logger, UnitofWork unitofWork) : BaseController
    {
        private readonly ILogger<AuthController> _logger = logger;
        private readonly UnitofWork _unitofWork = unitofWork;


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO newUser)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponse<string>(false, "Datos inválidos", null, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
                var userDto = await _unitofWork.UserRepository.RegisterUserAsync(newUser);
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

                var userDto = await _unitofWork.UserRepository.LoginUserAsync(loginDto);
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

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponse<string>(false, "Datos inválidos", null, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));


                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                                ?? throw new ArgumentNullException("User ID not found"));

                await _unitofWork.UserRepository.ChangePasswordAsync(changePasswordDto, userId);

                return Ok(new ApiResponse<string>(true, "Contraseña cambiada exitosamente"));
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error de validación al cambiar contraseña");
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Intento de cambio de contraseña con datos incorrectos");
                return Unauthorized(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al cambiar la contraseña");
                return StatusCode(500, new ApiResponse<string>(false, "Ocurrió un error inesperado"));
            }
        }
    }
}