using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiAcademy.DTOs;
using WebApiAcademy.Services;
using WebApiAcademy.Validations;

namespace WebApiAcademy.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService service)
        {
            _authService = service;
        }
        
        [HttpPost]
        public IActionResult Login([FromBody] UserCredentials credentials)
        {
            if(credentials == null)return BadRequest();
            if(string.IsNullOrEmpty(credentials.Email) || string.IsNullOrEmpty(credentials.Password))
            {
                return BadRequest(new { message = "Email y password son requeridos" });
            }
            
            if(!UserValidation.IsValidEmail(credentials.Email))
            {
                return BadRequest(new { message = "Email no válido" });
            }
            if(!UserValidation.IsValidPassword(credentials.Password))
            {
                return BadRequest(new { message = "correo o contraseña incorrectos" });
            }
            UserSession? user = _authService.Authenticate(credentials);
            if(user == null) return BadRequest(new { message = "Correo o contraseña incorrectos" });
            return Ok(user);
        }
        [HttpGet("token")]
        public IActionResult ValidateToken(string? Authorization)
        {
            string? token = Request.Headers["Authorization"];
            if(string.IsNullOrEmpty(token)) token ="Bearer "+ Authorization;
            if(string.IsNullOrEmpty(token)) return BadRequest(new { message = "Token no válido" });
            bool isValidToken = _authService.ValidateToken(token);
            if(!isValidToken) return BadRequest(new { message = "Token no válido" });
            string tokenString = _authService.RegenerateToken(token);
            UserSession? user = _authService.GetUserFromToken(tokenString);
            return Ok(user);
            
        }
    }
}
