using System.Net;
using Microsoft.AspNetCore.Mvc;
using Senior_Developer_Assessment.DTOs;
using Senior_Developer_Assessment.Models.Entities;
using Senior_Developer_Assessment.Models.Interfaces;

namespace Senior_Developer_Assessment.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{

    private readonly IUserService _UserService;
    private readonly ILogger<UserController> _logger;
    private readonly IAuthService _authService;
    private readonly string _message;

    public UserController(IUserService userService, IAuthService authService, ILogger<UserController> logger)
    {
        _UserService = userService;
        _authService = authService;
        _logger = logger;
        _message = "successfully";
    }

    // login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var (success, message, user) = await _authService.LoginAsync(request.Username, request.Password);

            if (!success)
                return Unauthorized(new { message });

            var token = _authService.GenerateJwtToken(user);
            var refreshToken = await _authService.GenerateRefreshTokenAsync(user, GetIpAddress());

            return Ok(new
            {   
                message = _message,
                info = Ok(new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                    user.Role,
                    Token = token,
                    RefreshToken = refreshToken
                })
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An error occurred during login" });
        }
    }


    // register a new user
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var (success, message, user) = await _authService.RegisterAsync(
                request.Username, request.Email, request.Password, request.Role);

            if (!success)
                return BadRequest(new { message });

            var token = _authService.GenerateJwtToken(user);
            var refreshToken = await _authService.GenerateRefreshTokenAsync(user, GetIpAddress());

            return Ok(new
            {   
                message = _message,
                info = Ok(new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                    user.Role,
                    Token = token,
                    RefreshToken = refreshToken
                })
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An error occurred during registration" });
        }
    }

    // Get all users
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            IEnumerable<UserDto> users = await _UserService.GetAllUsersAsync();

            return Ok(new { message = _message, Info = users});
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                new { message = "An error occurred while retrieving users" });
        }
    }



    private string GetIpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"];

        return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
    }
}

public class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}