using Microsoft.AspNetCore.Mvc;
using SecureTodoApi.Models.DTOs;
using SecureTodoApi.Services;
using SecureTodoApi.Security;

namespace SecureTodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtService _jwtService;

        public AuthController(IUserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest request)
        {
            var user = _userService.Register(request.Username, request.Password);

            if (user == null)
                return BadRequest(new { message = "Username already exists" });

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var user = _userService.ValidateUser(request.Username, request.Password);

            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            var token = _jwtService.GenerateToken(user);

            return Ok(new LoginResponse
            {
                Username = user.Username,
                Token = token
            });
        }
    }
}
