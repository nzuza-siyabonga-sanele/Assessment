using Microsoft.AspNetCore.Mvc;
using Senior_Developer_Assessment.Models.Interfaces;
using System.Net;

namespace Senior_Developer_Assessment.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;
    

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
        
    }

    // Endpoint to refresh JWT using refresh token
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        try
    {

        var (success, message, token, newRefreshToken) = 
            await _authService.RefreshTokenAsync(request.Token, GetIpAddress());

        if (!success)
            return Unauthorized(new { message });

        return Ok(new
        {
            nessage = "Token refreshed",
            Token = token,
            RefreshToken = newRefreshToken
        });
    }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An error occurred during token refresh" });
        }
    }

    // Endpoint to revoke a refresh token
    [HttpPost("revoke")]
    public async Task<IActionResult> Revoke([FromBody] RefreshTokenRequest request)
    {
        try
        {
            var success = await _authService.RevokeTokenAsync(request.Token, GetIpAddress());

            if (!success)
                return NotFound(new { message = "Token not found" });

            return Ok(new { message = "Token revoked", Token = request.Token });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token revocation");
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An error occurred during token revocation" });
        }
    }

    // Helper method to get IP address
    private string GetIpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"];

        return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
    }
}

// DTO for refresh token request
public class RefreshTokenRequest
{
    public string Token { get; set; } = string.Empty;
}