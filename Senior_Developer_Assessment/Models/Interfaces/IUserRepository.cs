
using Senior_Developer_Assessment.Models.Entities;
using Senior_Developer_Assessment.Models.Interfaces;


namespace TaskTracker.Core.Interfaces;

// Interface using user entities class for full information
public interface IUserRepository : IRepository<User>
{

    Task<User> GetByUsernameAsync(string username);
    Task<User> GetByEmailAsync(string email);
    Task<bool> UserExistsAsync(string username, string email);
}