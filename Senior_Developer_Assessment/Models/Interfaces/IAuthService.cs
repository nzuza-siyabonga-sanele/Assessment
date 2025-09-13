using Senior_Developer_Assessment.Models.Entities;

namespace Senior_Developer_Assessment.Models.Interfaces; 

// Interface for authentication and authorization services
public interface IAuthService
{
    // User registration and login methods
    Task<(bool Success, string Message, User User)> RegisterAsync(string username, string email, string password, UserRole role = UserRole.User);
    // User login method
    Task<(bool Success, string Message, User User)> LoginAsync(string username, string password);
    // Token generation and management methods
    string GenerateJwtToken(User user);
    // Generate refresh token
    Task<string> GenerateRefreshTokenAsync(User user, string ipAddress);
    // Refresh JWT using refresh token
    Task<(bool Success, string Message, string Token, string RefreshToken)> RefreshTokenAsync(string token, string ipAddress);
    // Revoke refresh token
    Task<bool> RevokeTokenAsync(string token, string ipAddress);
}