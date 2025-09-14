using Microsoft.EntityFrameworkCore;
using Senior_Developer_Assessment.Data;
using Senior_Developer_Assessment.Models.Entities;
using Senior_Developer_Assessment.Models.Repositories;


namespace Senior_Developer_Assessment.Models.Interfaces;

// Repository for user-specific data operations
public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(DataDbContext context) : base(context)
    {
    }

    // Get user by username
    public async Task<User> GetByUsernameAsync(string username) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

    // Get user by email
    public async Task<User> GetByEmailAsync(string email) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    // Check if user exists by username or email
    public async Task<bool> UserExistsAsync(string username, string email) =>
        await _context.Users.AnyAsync(u => u.Username == username || u.Email == email);

}