using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Senior_Developer_Assessment.Data;
using Senior_Developer_Assessment.Models.Entities;
using Senior_Developer_Assessment.Models.Interfaces;




namespace Senior_Developer_Assessment.Services;

// Service for authentication and authorization
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRepository<RefreshToken> _refreshTokenRepository;
    private readonly IConfiguration _configuration;
    private readonly DataDbContext _context;

    public AuthService(
        IUserRepository userRepository,
        IRepository<RefreshToken> refreshTokenRepository,
        IConfiguration configuration,
        DataDbContext context)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _configuration = configuration;
        _context = context;
    }

    // Register a new user
    public async Task<(bool Success, string Message, User User)> RegisterAsync(string username, string email, string password, UserRole role = UserRole.User)
    {
        // Check if user already exists
        if (await _userRepository.UserExistsAsync(username, email))
            return (false, "Username or email already exists", null);

        // Create password hash
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        // Create user
        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            Role = role
        };

        // Save user to database
        await _userRepository.AddAsync(user);
        await _userRepository.SaveAsync();

        return (true, "User registered successfully", user);
    }

    // Login user and return JWT token
    public async Task<(bool Success, string Message, User User)> LoginAsync(string username, string password)
    {
        // Find user by username and verify password
        var user = await _userRepository.GetByUsernameAsync(username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return (false, "Invalid credentials", null);

        return (true, "Login successful", user);
    }

    // Generate JWT token
    public string GenerateJwtToken(User user)
    {
        // token handler and key
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);

        // token descriptor with claims and expiry
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"])),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        // create token
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    // Generate refresh token
    public async Task<string> GenerateRefreshTokenAsync(User user, string ipAddress)
    {
        // create refresh token
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            UserId = user.Id
        };

        // save refresh token to database
        await _refreshTokenRepository.AddAsync(refreshToken);
        await _refreshTokenRepository.SaveAsync();

        return refreshToken.Token;
    }

    // Refresh JWT token using refresh token
    public async Task<(bool Success, string Message, string Token, string RefreshToken)> RefreshTokenAsync(string token, string ipAddress)
    {
        // find refresh token in database and check if active
        var currentRefreshToken = await _refreshTokenRepository.FindAsync(rt => rt.Token == token);

        if (currentRefreshToken == null || !currentRefreshToken.IsActive)
            return (false, "Invalid token", null, null);

        // Revoke current token
        currentRefreshToken.Revoked = DateTime.UtcNow;
        currentRefreshToken.RevokedByIp = ipAddress;
        _refreshTokenRepository.Update(currentRefreshToken);

        // Generate new token
        var user = await _userRepository.GetByIdAsync(currentRefreshToken.UserId);
        var newRefreshToken = await GenerateRefreshTokenAsync(user, ipAddress);
        var newJwtToken = GenerateJwtToken(user);

        await _refreshTokenRepository.SaveAsync();

        return (true, "Token refreshed successfully", newJwtToken, newRefreshToken);
    }

    // Revoke refresh token using refresh token
    public async Task<bool> RevokeTokenAsync(string token, string ipAddress)
    {
        // find refresh token in database and check if active
        var currentRefreshToken = await _refreshTokenRepository.FindAsync(rt => rt.Token == token);


        if (currentRefreshToken == null || !currentRefreshToken.IsActive)
            return false;

        // Revoke token
        currentRefreshToken.Revoked = DateTime.UtcNow;
        currentRefreshToken.RevokedByIp = ipAddress;
        _refreshTokenRepository.Update(currentRefreshToken);
        await _refreshTokenRepository.SaveAsync();

        return true;
    }
}