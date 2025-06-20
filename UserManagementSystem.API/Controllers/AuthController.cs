using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Application.DTOs;
using UserManagementSystem.Application.IServices;

namespace UserManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.AuthenticateAsync(loginDto);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return Unauthorized(result.Errors);
        }
    }
}
