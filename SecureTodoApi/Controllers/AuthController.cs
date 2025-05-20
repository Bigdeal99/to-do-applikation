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
            if (request.Password != request.ConfirmPassword)
                return BadRequest(new { message = "Passwords do not match" });

            var (user, errors) = _userService.Register(request.Username, request.Password);

            if (user == null)
                return BadRequest(new { message = "Registration failed", errors });

            return Ok(new { message = "User registered successfully" });
        }
        

       [HttpPost("login")]
public IActionResult Login(LoginRequest request)
{
    var result = _userService.ValidateUser(request.Username, request.Password);

    if (!result.IsSuccess)
    {
        if (result.IsLockedOut)
        {
            var until = result.User!.LockoutEndTime!.Value.ToLocalTime().ToString("HH:mm:ss");
            return Unauthorized(new { message = $"Account locked. Try again at {until}" });
        }

        return Unauthorized(new { message = "Invalid credentials" });
    }

    var user = result.User!;
    var token = _jwtService.GenerateToken(user);
    var refreshToken = _jwtService.GenerateRefreshToken();

    user.RefreshToken = refreshToken;
    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
    _userService.UpdateUser(user);

    return Ok(new LoginResponse
    {
        Username = user.Username,
        Token = token,
        RefreshToken = refreshToken
    });
}


        
        [HttpPost("refresh-token")]
    public IActionResult RefreshToken([FromBody] TokenRequest request)
    {
        var user = _userService.GetByRefreshToken(request.RefreshToken);
        if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            return Unauthorized(new { message = "Invalid or expired refresh token" });

        var newToken = _jwtService.GenerateToken(user);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        _userService.UpdateUser(user);

        return Ok(new LoginResponse
        {
            Username = user.Username,
            Token = newToken,
            RefreshToken = newRefreshToken
        });
    }
    }
}
