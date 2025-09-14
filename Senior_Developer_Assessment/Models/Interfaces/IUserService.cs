

using Senior_Developer_Assessment.DTOs;

namespace Senior_Developer_Assessment.Models.Interfaces;

// Interface using DTOs for security and display purposes
public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByUsernameAsync(string username);
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<bool> UserExistsAsync(string username, string email);
}
