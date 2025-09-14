

using Senior_Developer_Assessment.DTOs;
using Senior_Developer_Assessment.Models.Entities;
using Senior_Developer_Assessment.Models.Interfaces;

namespace Senior_Developer_Assessment.Services;

// Service implementation using DTOs for security and display purposes
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // Get all users
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync(); // fetches User entities
        return users.Select(u => MapToDto(u));
    }

    // Get user by username
    public async Task<UserDto?> GetUserByUsernameAsync(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        return user == null ? null : MapToDto(user);
    }

    // Get user by email
    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user == null ? null : MapToDto(user);
    }

    // Check if user exists by username or email
    public async Task<bool> UserExistsAsync(string username, string email)
    {
        return await _userRepository.UserExistsAsync(username, email);
    }

    // Map User entity to UserDto for secure data transfer and display
    private static UserDto MapToDto(User user) => new UserDto
    {
        Id = user.Id,
        Username = user.Username,
        Email = user.Email,
        Role = user.Role.ToString()
    };
}
