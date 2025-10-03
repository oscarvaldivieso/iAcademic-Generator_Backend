using iAcademicGenerator.BusinessLogic;
using iAcademicGenerator.BusinessLogic.Services;
using iAcademicGenerator.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace iAcademicGenerator.API.Controllers.AUTH
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthServices _authService;

        public AuthController(AuthServices authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        /// <summary>
        /// Login endpoint for students and teachers
        /// </summary>
        /// <param name="loginRequest">User credentials</param>
        /// <returns>JWT Token if credentials are valid</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequestDTO loginRequest)
        {
            try
            {
                var result = _authService.Login(loginRequest);

                // Retornar el código HTTP según el Type del ServiceResult
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult().Error($"Internal server error: {ex.Message}"));
            }
        }

        /// <summary>
        /// Validate if current token is still valid
        /// </summary>
        [HttpGet("validate")]
        [Authorize]
        public IActionResult ValidateToken()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var userType = User.FindFirst("tipo_usuario")?.Value;
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            return Ok(new
            {
                type = 200,
                code = 200,
                success = true,
                message = "Token is valid",
                data = new
                {
                    usu_codigo = userId,
                    usu_nombre = userName,
                    tipo_usuario = userType,
                    roles = string.Join(",", roles)
                }
            });
        }
    }
}
